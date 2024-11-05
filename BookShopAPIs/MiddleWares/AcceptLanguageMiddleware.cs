namespace BookShopAPIs.MiddleWares
{
    public class AcceptLanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AcceptLanguageMiddleware> _logger;

        public AcceptLanguageMiddleware(RequestDelegate next, ILogger<AcceptLanguageMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                _logger.LogInformation("Accept-Language header found: {AcceptLanguage}", acceptLanguage);
                context.Items["Accept-Language"] = acceptLanguage;
            }
            else
            {
                _logger.LogWarning("No Accept-Language header found for request from IP: {ClientIp}", context.Connection.RemoteIpAddress?.ToString());
            }

            await _next(context);
        }
    }
}
