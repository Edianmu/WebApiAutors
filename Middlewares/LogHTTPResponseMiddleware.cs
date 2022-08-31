using Microsoft.Extensions.Logging;

namespace WebApiAutors.Middlewares
{
    public class LogHTTPResponseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LogHTTPResponseMiddleware> logger;

        public LogHTTPResponseMiddleware(RequestDelegate next,
            ILogger<LogHTTPResponseMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        //Invoke or invokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream()) //ms to save http responses in log
            {
                var bodyOriginalResponse = context.Response.Body;
                context.Response.Body = ms;

                await next(context);

                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(bodyOriginalResponse);
                context.Response.Body = bodyOriginalResponse;

                logger.LogInformation(response);
            }
        }
    }
}
