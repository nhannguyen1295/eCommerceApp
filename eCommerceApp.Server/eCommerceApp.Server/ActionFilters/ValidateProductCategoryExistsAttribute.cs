using System;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceApp.Server.ActionFilters
{
    public class ValidateProductCategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;

        public ValidateProductCategoryExistsAttribute(IRepositoryManager repositoryManager,
                                                      ILoggerManager logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var categoryId = (Guid)context.ActionArguments["categoryId"];
            var productId = (Guid)context.ActionArguments["productId"];
            var productCategory = await _repositoryManager.ProductCategory.GetProductCategoryAsync(categoryId,
                                                                                                   productId,
                                                                                                   trackChanges);

            if (productCategory is null)
            {
                _logger.LogInfo($"Does not exists categoryID: {categoryId} associated with productId: {productId}");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("productCategory", productCategory);
                await next();
            }
        }
    }
}