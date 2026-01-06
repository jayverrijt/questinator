using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Questinator.AI.Security
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices
                .GetRequiredService<IConfiguration>();

            var expectedKey = config["AiSecurity:ApiKey"];
            var providedKey = context.HttpContext.Request.Headers["X-API-KEY"].FirstOrDefault();

            if (string.IsNullOrEmpty(providedKey) || providedKey != expectedKey)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Invalid or missing API key"
                );
                return;
            }

            await next();
        }
    }
}