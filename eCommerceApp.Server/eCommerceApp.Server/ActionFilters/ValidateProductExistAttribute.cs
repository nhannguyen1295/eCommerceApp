using System;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceApp.Server.ActionFilters
{
    public class ValidateProductExistAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;

        public ValidateProductExistAttribute(IRepositoryManager repositoryManager, ILoggerManager logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var productId = (Guid)context.ActionArguments["productId"];
            var product = await _repositoryManager.Product.GetProductAsync(productId, trackChanges);

            if (product is null)
            {
                _logger.LogInfo($"Product with id: {productId} does not exist in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("product", product);
                await next();
            }
        }
    }
}