using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopAPIs.MiddleWares
{
    public class ProfilingMiddleware
    {
        private readonly ILogger<ProfilingMiddleware> _logger;
        private readonly RequestDelegate _Next;
        public ProfilingMiddleware(RequestDelegate _requestDelegate , ILogger<ProfilingMiddleware> _logger) 
        {
            _Next = _requestDelegate;
            this._logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _Next(context);
            stopwatch.Stop();
            _logger.LogInformation($"Request '{context.Request.Path}' took '{stopwatch.ElapsedMilliseconds}ms' To Execute");
        }

    }
}
