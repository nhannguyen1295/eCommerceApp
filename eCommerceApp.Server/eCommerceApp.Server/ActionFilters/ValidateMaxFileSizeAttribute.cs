using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceApp.Server.ActionFilters
{
    public class ValidateMaxFileSizeAttribute : IAsyncActionFilter
    {
        private const int MAX_FILE_LENGTH = 10485760;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var files = context.HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > MAX_FILE_LENGTH)
                    {
                        context.Result = new BadRequestObjectResult($"Maximum allowed file size is {MAX_FILE_LENGTH} bytes");
                        return;
                    }
                }
                await next();
            }
            else
            {
                context.Result = new BadRequestObjectResult($"File must be not empty");
            }
        }
    }
}