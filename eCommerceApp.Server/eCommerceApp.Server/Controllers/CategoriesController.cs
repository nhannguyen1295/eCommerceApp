using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerceApp.Contract;
using eCommerceApp.Entities.DTO;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.RequestFeatures;
using eCommerceApp.Server.ActionFilters;
using eCommerceApp.Server.ModelBinders;
using eCommerceApp.Server.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eCommerceApp.Server.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/v{v:apiversion}/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        private readonly CategoryLinks _categoryLinks;

        public CategoriesController(ICategoryService categoryService,
                                    ILoggerManager loggerManager,
                                    IMapper mapper,
                                    CategoryLinks dataShaper)
        {
            _categoryService = categoryService;
            _loggerManager = loggerManager;
            _mapper = mapper;
            _categoryLinks = dataShaper;
        }

        /// <summary>
        /// Get the list of all Categories
        /// </summary>
        /// <param name="categoryParameters"></param>
        /// <returns>The categories list</returns>
        /// <response code="200">Returns the list of all categories</response>
        /// <response code="400">If wrong API version</response>
        [HttpGet(Name = "GetCategories")]
        [HttpHead]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryParameters categoryParameters)
        {
            var categories = await _categoryService.GetCategoriesAsync(categoryParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categories.MetaData));

            var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            var links = _categoryLinks.TryGenerateLinks(categoriesDTO, categoryParameters.Fields, HttpContext);
            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }

        /// <summary>
        /// Get a category by Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>A category</returns>
        /// <response code="200">Returns category</response>
        /// <response code="404">If categoryId does not exist in the database</response>
        [HttpGet("{categoryId}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _loggerManager.LogInfo($"Category with id: {categoryId} does not exist in the database");
                return NotFound();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        /// <summary>
        /// Creates a newly created category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A newly created category</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCategory")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryForCreationDTO category)
        {
            if (category.ParentCategoryId != null)
            {
                var categoryExisted = await _categoryService.GetCategoryAsync((Guid)category.ParentCategoryId, false);
                if (categoryExisted == null)
                {
                    _loggerManager.LogError($"Category with id: {category.ParentCategoryId} does not exist in the database");
                    return NotFound("Parent ID does not existed");
                }
            }

            var categoryEntity = _mapper.Map<Category>(category);
            _categoryService.CreateCategory(categoryEntity);
            await _categoryService.SaveAsync();

            var categoryToReturn = _mapper.Map<CategoryDTO>(categoryEntity);

            return CreatedAtRoute("CategoryById", new { categoryId = categoryToReturn.Id }, categoryToReturn);
        }

        /// <summary>
        /// Get the collection of categories
        /// <para>URL from swaggerUI is ERR. Please use POSTMAN to test</para>
        /// <para>Exact URL: https://localhost:5001/api/1/categories/collection/(id1, id2,..idn)</para>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Collection of categories</returns>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If one or more Id does not exist in the database</response>
        [HttpGet("collection/({ids})", Name = "CategoryCollection")]
        public async Task<IActionResult> GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _loggerManager.LogError("Parameter ids is NULL");
                return BadRequest("Parameter ids is NULL");
            }

            var categoryEntities = await _categoryService.GetByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != categoryEntities.Count())
            {
                _loggerManager.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntities);
            return Ok(categoriesToReturn);
        }

        /// <summary>
        /// Creates newly created category collection
        /// </summary>
        /// <param name="categoryCollection"></param>
        /// <returns>List of newly created categories</returns>
        ///<response code="201">Returns the newly created items collection</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategoryCollection([FromBody] IEnumerable<CategoryForCreationDTO> categoryCollection)
        {
            var categoryEntities = _mapper.Map<IEnumerable<Category>>(categoryCollection);
            foreach (var category in categoryEntities)
            {
                _categoryService.CreateCategory(category);
            }

            await _categoryService.SaveAsync();

            var categoryCollectionToReturn = _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntities);
            var ids = string.Join(",", categoryCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("CategoryCollection", new { ids }, categoryCollectionToReturn);
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns>No content</returns>
        /// <response code="204">Update completed</response>
        /// <response code="404">Id does not exist in the database</response>
        [HttpPut("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryForUpdateDTO category)
        {
            var categoryEntity = HttpContext.Items["category"] as Category;
            categoryEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            _mapper.Map(category, categoryEntity);
            await _categoryService.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete category by Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>No content</returns>
        /// <response code="404">Id does not exist in the database</response>
        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var category = HttpContext.Items["category"] as Category;

            _categoryService.DeleteCategory(category);
            await _categoryService.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Partially update category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>No content</returns>
        /// <response code="404">Id does not exist in the database</response>
        /// <response code="204">Update completed</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCategory(Guid categoryId,
                                                                 [FromBody] JsonPatchDocument<CategoryForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                _loggerManager.LogError("patchDoc object sent from client is NULL");
                return BadRequest("patchDoc object is NULL");
            }

            var categoryEntity = HttpContext.Items["category"] as Category;
            categoryEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            var categoryToPatch = _mapper.Map<CategoryForUpdateDTO>(categoryEntity);
            patchDoc.ApplyTo(categoryToPatch, ModelState);
            TryValidateModel(categoryToPatch);
            if (!ModelState.IsValid)
            {
                _loggerManager.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(categoryToPatch, categoryEntity);
            await _categoryService.SaveAsync();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCategoriesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }
    }
}