using Ecommerce.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecommerce.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache cache)
        {
            _next = next;
            _environment = environment;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new APIException(
                        (int)HttpStatusCode.TooManyRequests,
                        "Too many requests, please try again later"
                    );

                    await context.Response.WriteAsJsonAsync(response);

                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment()
                    ? new APIException(
                        (int)HttpStatusCode.InternalServerError,
                        ex.Message ?? string.Empty,
                        ex.StackTrace ?? string.Empty)
                    : new APIException(
                        (int)HttpStatusCode.InternalServerError,
                        ex.Message ?? string.Empty);

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var cacheKey = $"Rate:{ip}";

            var data = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;

                return (TimeStamp: DateTime.UtcNow, Count: 0);
            });

            if (data.Count >= 50)
            {
                return false;
            }

            _cache.Set(
                cacheKey,
                (data.TimeStamp, data.Count + 1),
                _rateLimitWindow
            );

            return true;
        }

        private static bool ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Production"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            return true;
        }
    }
}