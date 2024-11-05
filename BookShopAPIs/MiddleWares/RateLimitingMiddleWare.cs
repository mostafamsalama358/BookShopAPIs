using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.RateLimiting;

namespace BookShopAPIs.MiddleWares
{
    public class RateLimitingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, TokenBucketRateLimiter> _limiters = new();
        private readonly ILogger<RateLimitingMiddleWare> _logger;

        public RateLimitingMiddleWare(RequestDelegate next , ILogger<RateLimitingMiddleWare> _logger)
        {
            _next = next;
            this._logger = _logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var limiter = _limiters.GetOrAdd(clientIp, _ => new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
            {
                TokenLimit = 5,                // Maximum requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10), // Reset period
                TokensPerPeriod = 5,           // Requests allowed per period
                AutoReplenishment = true
            }));



            using var lease = await limiter.AcquireAsync(1);

            if (!lease.IsAcquired)
            {
                // Log the rate limit exceeded event
                _logger.LogWarning("Rate limit exceeded for IP: {ClientIp}", clientIp);

                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                _logger.LogInformation($"Request '{context.Request.Path}' took ");
                return;
            }


            await _next(context);
            }
    }
}
