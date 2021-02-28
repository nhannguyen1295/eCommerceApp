using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceApp.Server.ActionFilters
{
    public class ValidateFileExtensionAttribute : IAsyncActionFilter
    {
        private readonly string[] FILE_EXTENSION_ALLOWED = { "jpg", "png", "jpeg", "gif", "flv", "mp4" };
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var files = context.HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    var fileExtension = Path.GetExtension(file.FileName).Substring(1).ToLower();
                    if (!FILE_EXTENSION_ALLOWED.Contains(fileExtension))
                    {
                        context.Result = new BadRequestObjectResult($"Allowed extension: jpg, png, jpeg, gif, flv, mp4");
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