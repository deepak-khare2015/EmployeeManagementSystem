using EmployeeManagement.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.MIddleware
{
    //Global Exception handling middleware
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //If any exxception is thrown handle here
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task  HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = Guid.NewGuid().ToString();

            //Default Response
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occoured. PLease try again later";

            //Handle known exception
            if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is ArgumentException || exception is ArgumentNullException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;

            }

            // Add correlationID in header
            context.Response.Headers["X-Correlation-Id"] = correlationId;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            //Convert error object to json (client will get json format response)
            var payload = JsonSerializer.Serialize(new
            {
                correlationId ,
                statusCode,
                message

            });

            return context.Response.WriteAsync(payload);
                
        }
    }
}
