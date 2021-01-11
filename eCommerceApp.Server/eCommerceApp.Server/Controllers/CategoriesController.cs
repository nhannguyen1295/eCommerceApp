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
    [Route("api/{v:apiversion}/categories")]
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

        [ResponseCache(Duration = 120)]
        [HttpGet(Name = "GetCategories")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryParameters categoryParameters)
        {
            var categories = await _categoryService.GetCategoriesAsync(categoryParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categories.MetaData));

            var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            var links = _categoryLinks.TryGenerateLinks(categoriesDTO, categoryParameters.Fields, HttpContext);
            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }

        [HttpGet("{id}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryAsync(id, trackChanges: false);
            if (category == null)
            {
                _loggerManager.LogInfo($"Category with id: {id} does not exist in the database");
                return NotFound();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

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

            return CreatedAtRoute("CategoryById", new { id = categoryToReturn.Id }, categoryToReturn);
        }

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

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryForUpdateDTO category)
        {
            var categoryEntity = HttpContext.Items["category"] as Category;
            categoryEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            _mapper.Map(category, categoryEntity);
            await _categoryService.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = HttpContext.Items["category"] as Category;

            _categoryService.DeleteCategory(category);
            await _categoryService.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCategory(Guid id, [FromBody] JsonPatchDocument<CategoryForUpdateDTO> patchDoc)
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