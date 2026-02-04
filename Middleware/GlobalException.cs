using System.Net;
using System.Text.Json;
using ComputerSeekho.API.Exceptions;

namespace ComputerSeekho.API.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case BadRequestException:
                    status = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Something went wrong!";
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                statusCode = (int)status
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(result);
        }
    }
}
