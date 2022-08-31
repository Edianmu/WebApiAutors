using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutors.Filters
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la acción");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Después de ejecutar la acción");
        }


    }
}
