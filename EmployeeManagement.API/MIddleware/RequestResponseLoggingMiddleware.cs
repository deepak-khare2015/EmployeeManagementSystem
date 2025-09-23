using Microsoft.AspNetCore.Http;
using Serilog; // NEW
using Serilog.Context;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.API.MIddleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1️⃣ Generate or reuse CorrelationId (unique per request)
            //var correlationId = context.Items.TryGetValue("CorrelationId", out var cidObj)
            //    ? cidObj?.ToString()
            //    : Guid.NewGuid().ToString();

            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-Id", out var cidObj)
                ? cidObj.ToString()
                : Guid.NewGuid().ToString("N"); // e.g., "e4c8f8d2b1f34d8a8f7f9d24b03a8e1f" //
                                                // 👉 In production, many teams prefer a shorter format (no dashes) to make it easier to read/search.

            // Save in HttpContext.Items so controllers/services can access it
            context.Items["CorrelationId"] = correlationId;


            // 2️⃣ Add CorrelationId to response header so client also gets it
            context.Response.Headers["X-Correlation-Id"] = correlationId;

            // 3️⃣ Push CorrelationId into Serilog LogContext
            //    🔹 From now on, ALL logs (Controller, Service, Middleware) 
            //    🔹 automatically include CorrelationId, no need to pass it manually
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                // 🔹 Log incoming request
                await LogRequest(context);


                // --- Capture response body ---
                var originalBodyStream = context.Response.Body;
                await using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                var exceptionThrown = false;
                try
                {
                    await _next(context); // ✅ Continue to next middleware / Controller
                }
                catch (Exception ex)
                {
                    exceptionThrown = true;
                    // 🔹 Exception will include CorrelationId automatically (thanks to LogContext)
                    Log.Error(ex, "Unhandled exception occurred");

                    // restore original stream so ExceptionMiddleware can write proper JSON response
                    context.Response.Body = originalBodyStream;
                    throw; // rethrow to be handled by ExceptionMiddleware
                }

                // --- Log outgoing response ---
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                if (!string.IsNullOrWhiteSpace(responseText))
                    Log.Information("Response: {StatusCode} {Body}",
                        context.Response.StatusCode, responseText);
                else
                    Log.Information("Response: {StatusCode} (no body)",
                        context.Response.StatusCode);

                // Copy response back to pipeline
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering(); // ✅ allows reading request body multiple times
            string bodyText = string.Empty;

            if (context.Request.ContentLength is > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                bodyText = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // ✅ reset so controller can read it again
            }

            // 🔹 CorrelationId is auto-attached to this log via LogContext
            Log.Information("Request: {Method} {Path} {Body}",
                context.Request.Method, context.Request.Path, bodyText);
        }
    }
}
