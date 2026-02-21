using LearnixCRM.Application.Common.Responses;
using System.Net;
using System.Text.Json;

namespace LearnixCRM.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleException(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, ex, HttpStatusCode.NotFound);
            }
            catch (ArgumentException ex)
            {
                await HandleException(context, ex, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                await HandleException(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleException( HttpContext context,Exception exception,HttpStatusCode statusCode)
        {
            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ApiResponse<object>
            {
                IsSuccess = false,
                Message = exception.Message,
                StatusCode = (int)statusCode,
                Data = null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
