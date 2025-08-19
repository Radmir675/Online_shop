using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OnlineShopWebApp.MiddleWare
{
    public class TaskCancellationTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TaskCancellationTokenMiddleware> logger;

        public TaskCancellationTokenMiddleware(RequestDelegate next, ILogger<TaskCancellationTokenMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception _) when (_ is OperationCanceledException or TaskCanceledException)
            {
                var requestPath = httpContext.Request.Path;
                logger.LogInformation($"Request \"{requestPath}\" is cancelled");
            }

        }
    }
}
