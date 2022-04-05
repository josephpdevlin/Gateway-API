using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Gateway.Api.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyName = "api-key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var apiKey);

            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKeyFromConfig = config.GetValue<string>(ApiKeyName);

            if(apiKey != apiKeyFromConfig)
            {
                context.Result = new ContentResult()
                {
                    Content = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            if(string.IsNullOrEmpty(apiKey))
            {
                context.Result = new ContentResult()
                {
                    Content = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }
            await next();
        }

    }
}
