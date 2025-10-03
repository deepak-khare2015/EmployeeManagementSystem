using System.Text.Json;

namespace EmployeeManagement.API.MIddleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Let pipeline continue first
            await _next(context);

            if (context.Items.TryGetValue("ModelStateErrors", out var errorsObj)
&& errorsObj is IEnumerable<object> errorsEnumerable)
            {
                var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString("N");

                var response = new
                {
                    correlationId,
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Validation failed",
                    details = errorsEnumerable
                };

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }


        }
    }
}

