using Microsoft.AspNetCore.Http;
using Serilog; // NEW
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
            // Correlation Id propagated via Items and header for downstream use
            var correlationId = context.Items.TryGetValue("CorrelationId", out var cidObj)
                ? cidObj?.ToString()
                : Guid.NewGuid().ToString();

            context.Response.Headers["X-Correlation-Id"] = correlationId;

            // Log Request
            await LogRequest(context, correlationId);

            // Swap response body to buffer
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var exceptionThrown = false;
            try
            {
                await _next(context); // continue pipeline
            }
            catch (Exception ex)
            {
                exceptionThrown = true;                       // mark that exception occurred
                Log.Error(ex, "Exception for {CorrelationId}", correlationId); // log but DO NOT write any body here
                context.Response.Body = originalBodyStream;   // restore immediately so exception middleware can write
                throw;                                        // rethrow to ExceptionMiddleware
            }

            // If we reach here, no exception middleware handled the response body.
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrWhiteSpace(responseText))
                Log.Information("Response {CorrelationId}: {StatusCode} {Body}",
                    correlationId, context.Response.StatusCode, responseText);
            else
                Log.Information("Response {CorrelationId}: {StatusCode} (no body)",
                    correlationId, context.Response.StatusCode);

            // Copy back only if response hasn’t already started and no exception path
            if (!exceptionThrown && !context.Response.HasStarted)
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
            else
            {
                // Defensive: if response started, just flush what’s there
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task LogRequest(HttpContext context, string? correlationId)
        {
            context.Request.EnableBuffering(); // allows reading request body multiple times
            string bodyText = string.Empty;

            //var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            //context.Request.Body.Position = 0;

            //// Mask sensitive fields manually (simple example) if you want to hide password or data in logs
            //if (requestBody.Contains("password", StringComparison.OrdinalIgnoreCase))
            //{
            //    //requestBody = requestBody.Replace(
            //    //    requestBody.Substring(requestBody.IndexOf("password"), 20),
            //    //    "\"password\":\"***MASKED***\"");
            //    // fallback simple string replace if body isn't JSON
            //    requestBody = requestBody.Replace("salary", "\"salary\":\"***MASKED***\"", StringComparison.OrdinalIgnoreCase);
            //}


            if (context.Request.ContentLength is > 0)
            {
                using var reader = new StreamReader(
                    context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                bodyText = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            Log.Information("Request {CorrelationId}: {Method} {Path} {Body}",
                correlationId, context.Request.Method, context.Request.Path, bodyText);
        }

    }
}
