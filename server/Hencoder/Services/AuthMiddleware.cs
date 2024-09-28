using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using ZeroLevel;

namespace Hencoder.Services
{
    public static class AuthMiddlewareExtension
    {
        public static IApplicationBuilder UseAuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }

    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ReadDataFromContext(context);
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var controllerActionDescriptor = context?
                .GetEndpoint()?
                .Metadata?
                .GetMetadata<ControllerActionDescriptor>();
                var controllerName = controllerActionDescriptor?.ControllerName ?? string.Empty;
                var actionName = controllerActionDescriptor?.ActionName ?? string.Empty;
                Log.Error(ex, $"[{controllerName}.{actionName}]");
                if (context != null)
                {
                    switch (ex)
                    {
                        case KeyNotFoundException
                            or FileNotFoundException:
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;

                        case UnauthorizedAccessException:
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            break;

                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                    }
                    await context.Response.WriteAsync(ex.Message ?? "Error");
                }
            }
        }

        private void ReadDataFromContext(HttpContext context)
        {
            var token = context.Request.Headers["X-Token"];
            var op_context = new OperationContext(token!);
            context.Items["op_context"] = op_context;
        }
    }
}
