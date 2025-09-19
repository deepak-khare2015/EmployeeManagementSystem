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
            //Log Request
            context.Request.EnableBuffering(); //Enable buffering to read the request body multiple times
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0; //Reset the stream position to allow further reading by other middlewares
            Log.Information("Incoming Request: {method} {url} {body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);
            //Copy original response body stream
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context); //Call the next middleware
                //Log Response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                Log.Information("Outgoing Response: {statusCode} {body}",
                    context.Response.StatusCode,
                    responseText);
                //Copy the contents of the new memory stream (which contains the response) to the original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
