using System;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerceApp.Server.ActionFilters
{
    public class ValidateCategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateCategoryExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var id = (Guid)context.ActionArguments["id"];
            var category = await _repository.Category.GetCategoryAsync(id, trackChanges);

            if (category == null)
            {
                _logger.LogInfo($"Category with id: {id} does not exist in the database");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("category", category);
                await next();
            }
        }
    }
}