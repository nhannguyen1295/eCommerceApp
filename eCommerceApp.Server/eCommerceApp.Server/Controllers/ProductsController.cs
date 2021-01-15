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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eCommerceApp.Server.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("/api/v{v:apiversion}/categories/{categoryId}/products")]

    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductDTO> _dataShaper;

        public ProductsController(IProductService productService,
                                  ILoggerManager loggerManager,
                                  IMapper mapper,
                                  IDataShaper<ProductDTO> dataShaper)
        {
            _productService = productService;
            _loggerManager = loggerManager;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet(Name = "GetProductsForCategory")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> GetProductsForCategory(Guid categoryId,
                                                                [FromQuery] ProductParameters productParameters)
        {
            var productCategories = await _productService.GetProductsForCategoryAsync(categoryId, trackChanges: false);
            var products = await _productService.GetProductsAsync(productParameters, trackChanges: false);
            var productsFilter = products.Where(x => productCategories.Any(productCategory => productCategory.ProductId == x.Id));
            var productsFilterToPagedList = PagedList<Product>.ToPagedList(productsFilter,
                                                                           productParameters.PageNumber,
                                                                           productParameters.PageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(productsFilterToPagedList.MetaData));

            var productDTO = _mapper.Map<IEnumerable<ProductDTO>>(productsFilterToPagedList);

            return Ok(_dataShaper.ShapeData(productDTO, productParameters.Fields));
        }

        [HttpGet("{productId}", Name = "GetProductForCategory")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> GetProductForCategory(Guid categoryId, Guid productId)
        {
            var product = await _productService.GetProductAsync(productId, trackChanges: false);
            if (product is null)
            {
                _loggerManager.LogInfo($"Product with id: {productId} does not exist in the database");
                return NotFound();
            }
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }

        [HttpPost(Name = "CreateProduct")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> CreateProductForCategory(Guid categoryId, [FromBody] ProductForCreationDTO product)
        {
            var productEntity = _mapper.Map<Product>(product);
            await _productService.CreateProductForCategory(categoryId, productEntity);

            var productToReturn = _mapper.Map<ProductDTO>(productEntity);
            return CreatedAtRoute("GetProductForCategory",
                                  new { categoryId, productId = productToReturn.Id }, productToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ProductCollection")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> GetProductCollection(Guid categoryId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _loggerManager.LogError("Parameter ids is NULL");
                return BadRequest("Parameter ids is NULL");
            }
            var productEntities = await _productService.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != productEntities.Count())
            {
                _loggerManager.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var productCategory = await _productService.GetProductsForCategoryAsync(categoryId, trackChanges: false);
            var productFilter = productEntities.Where(x => productCategory.Any(productCategory => productCategory.ProductId == x.Id));

            if (ids.Count() != productFilter.Count())
            {
                _loggerManager.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var productToReturn = _mapper.Map<IEnumerable<ProductDTO>>(productFilter);
            return Ok(productToReturn);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> CreateProductCollectionForCategory(Guid categoryId, [FromBody] IEnumerable<ProductForCreationDTO> productsCollection)
        {
            var productEntities = _mapper.Map<IEnumerable<Product>>(productsCollection);
            foreach (var product in productEntities)
            {
                await _productService.CreateProductForCategory(categoryId, product);

            }

            var productCollectionToReturn = _mapper.Map<IEnumerable<ProductDTO>>(productEntities);
            var ids = string.Join(",", productCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("ProductCollection", new { categoryId, ids }, productCollectionToReturn);
        }

        [HttpDelete("{productId}")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteProductForCategory(Guid categoryId, Guid productId)
        {
            var product = await _productService.GetProductAsync(productId, trackChanges: false);

            _productService.DeleteProduct(product);
            await _productService.SaveAsync();

            return NoContent();
        }

        [HttpPut("{productId}")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateProductForCategory(Guid categoryId,
                                                                  Guid productId,
                                                                  [FromBody] ProductForUpdateDTO product)
        {
            var productEntity = await _productService.GetProductAsync(productId, trackChanges: true);
            productEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            _mapper.Map(product, productEntity);

            await _productService.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{productId}")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid categoryId,
                                                                Guid productId,
                                                                [FromBody] JsonPatchDocument<ProductForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                _loggerManager.LogError("patchDoc object sent from client is NULL");
                return BadRequest("patchDoc object is NULL");
            }
            var productEntity = await _productService.GetProductAsync(productId, trackChanges: true);
            productEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime();

            var productToPatch = _mapper.Map<ProductForUpdateDTO>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            TryValidateModel(productToPatch);
            if (!ModelState.IsValid)
            {
                _loggerManager.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(productToPatch, productEntity);
            await _productService.SaveAsync();
            return NoContent();
        }


        [HttpGet("/products/{productId}/categories", Name = "GetCategoriesForProduct")]
        [ServiceFilter(typeof(ValidateProductExistAttribute))]
        public async Task<IActionResult> GetCategoriesForProduct(Guid productId)
        {
            var productCategories = await _productService.GetCategoriesForProductAsync(productId, trackChanges: false);
            var categories = await _productService.GetCategoriesAsync(trackChanges: false);
            var categoriesFilter = categories.Where(x => productCategories.Any(y => y.CategoryId == x.Id));
            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDTO>>(categoriesFilter);
            return Ok(categoriesToReturn);
        }

        //!!! Pausing
        // [HttpPost("/products/{productId}/categories/collection")]
        // [ServiceFilter(typeof(ValidateProductExistAttribute))]
        // public async Task<IActionResult> CreateCategoryCollectionForProduct(Guid productId,
        //                                                                     [FromBody] IEnumerable<CategoryForCreationDTO> categories)
        // {
        //     var categoryEntities = _mapper.Map<IEnumerable<Category>>(categories);
        //     var categoriesDB = await _productService.GetCategoriesAsync(trackChanges: false);
        //     var setToRemove = new HashSet<Category>(categoriesDB.Where(x => categoryEntities.Any(y => y.Name == x.Name)));
        //     categoryEntities.ToList().RemoveAll(x => setToRemove.Contains(x));

        //     foreach (var category in categoryEntities)
        //     {
        //         _productService.CreateCategory(category);
        //         await _productService.SaveAsync();
        //         _productService.CreateProductCategory(productId, category.Id);
        //     }

        //     await _productService.SaveAsync();
        //     return NoContent();
        // }
    }
}