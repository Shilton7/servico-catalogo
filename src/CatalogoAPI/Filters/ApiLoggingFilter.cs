using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace CatalogoAPI.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Starting Log");
            _logger.LogInformation($"Datetime Start: {DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Controller: { context.Controller }");
            _logger.LogInformation($"Path Request: { context.HttpContext.Request.Path }");
            _logger.LogInformation($"ModelState is Valid: { context.ModelState.IsValid }");
           
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Finishing Log");
            _logger.LogInformation($"Result: { context.Result }");
            _logger.LogInformation($"Datetime Finish: {DateTime.Now.ToLongTimeString()}");
        }

    }
}
