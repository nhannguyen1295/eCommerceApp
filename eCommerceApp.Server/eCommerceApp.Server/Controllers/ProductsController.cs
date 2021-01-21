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

        /// <summary>
        /// Get a list of Product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productParameters"></param>
        /// <returns>The list of product associated with categoryId</returns>
        /// <response code="200">Returns the list of all categories</response>
        /// <response code="404">CategoryId does not exist in the database</response>
        [HttpGet(Name = "GetProductsForCategory")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> GetProductsForCategory(Guid categoryId,
                                                                [FromQuery] ProductParameters productParameters)
        {
            if (!productParameters.ValidPriceRange)
            {
                return BadRequest("Max price cannot be less than min age.");
            }
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

        /// <summary>
        /// Get a product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <returns>A product</returns>
        /// <response code="200">Return a product</response>
        /// <response code="404">CategoryId or ProductId does not exist in the database or them does not associated</response>
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

        /// <summary>
        /// Create a newly created product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="product"></param>
        /// <returns>A newly created product</returns>
        /// <response code="201">Returns the list of all newly created products</response>
        /// <response code="404">If CategoryId does not exist in the database</response>
        /// <response code="422">If model invalid</response>
        [HttpPost(Name = "CreateProduct")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> CreateProductForCategory(Guid categoryId, [FromBody] ProductForCreationDTO product)
        {
            var productEntity = _mapper.Map<Product>(product);
            await _productService.CreateProductForCategoryAsync(categoryId, productEntity);

            var productToReturn = _mapper.Map<ProductDTO>(productEntity);
            return CreatedAtRoute("GetProductForCategory",
                                  new { categoryId, productId = productToReturn.Id }, productToReturn);
        }

        /// <summary>
        /// Get a collection of product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="ids"></param>
        /// <returns>The collection of product</returns>
        ///  <response code="200">Returns the collection of newly created products</response>
        ///  <response code="404">If categoryId or productId does not exist in the database</response>
        ///  <response code="400">If parameter ids is NULL</response>
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

        /// <summary>
        /// Create a collection of new product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productsCollection"></param>
        /// <returns>A collection of newly created products</returns>
        ///  <response code="201">Returns the collection of newly created products</response>
        /// <response code="400">If parameter ids is NULL</response>
        /// <response code="404">If categoryId does not exist in the database</response>
        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> CreateProductCollectionForCategory(Guid categoryId, [FromBody] IEnumerable<ProductForCreationDTO> productsCollection)
        {
            var productEntities = _mapper.Map<IEnumerable<Product>>(productsCollection);
            foreach (var product in productEntities)
            {
                await _productService.CreateProductForCategoryAsync(categoryId, product);

            }

            var productCollectionToReturn = _mapper.Map<IEnumerable<ProductDTO>>(productEntities);
            var ids = string.Join(",", productCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("ProductCollection", new { categoryId, ids }, productCollectionToReturn);
        }

        /// <summary>
        /// Delete a product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <returns>No content</returns>
        /// <response code="404">CategoryId or ProductId does not exist in the database or them does not associated</response>
        [HttpDelete("{productId}")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteProductForCategory(Guid categoryId, Guid productId)
        {
            var product = await _productService.GetProductAsync(productId, trackChanges: false);

            _productService.DeleteProduct(product);
            await _productService.SaveAsync();

            return NoContent();
        }

        /// <summary>
        /// Update product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns>No content</returns>
        /// <response code="404">CategoryId or ProductId does not exist in the database or them does not associated</response>
        /// <response code="405">If productID is NULL</response>
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

        /// <summary>
        /// Partially update product for category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="patchDoc"></param>
        /// <returns>No content</returns>
        /// <response code="404">CategoryId or ProductId does not exist in the database or them does not associated</response>
        /// <response code="405">If productID is NULL</response>
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

        /// <summary>
        /// Get categories for product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>The list of categories for product</returns>
        /// <response code="404">CaProductId does not exist in the database</response>
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
        [HttpPost("/v{v:apiversion}/products/{productId}/categories/collection")]
        [ServiceFilter(typeof(ValidateProductExistAttribute))]
        public async Task<IActionResult> CreateCategoryCollectionForProduct(Guid productId,
                                                                            [FromBody] IEnumerable<CategoryForProductUpdateDTO> categories)
        {
            var categoryEntities = _mapper.Map<IEnumerable<Category>>(categories);
            var categoriesDB = await _productService.GetCategoriesAsync(trackChanges: false);
            foreach (var category in categoryEntities)
            {
                if (categoriesDB.Any(x => x.Name == category.Name))
                {
                    _productService.CreateProductCategory(category.Id, productId);
                    await _productService.SaveAsync();
                }
                else
                {
                    await _productService.CreateCategoryForProductAsync(productId, category);
                }
            }

            return NoContent();
        }
    }
}