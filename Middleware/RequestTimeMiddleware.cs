
using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var ellapsedMilisecond = _stopWatch.ElapsedMilliseconds;

            if (ellapsedMilisecond / 1000 > 4)
            {
                var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {ellapsedMilisecond} ms";
                _logger.LogInformation(message);
            }
        }
    }
}
