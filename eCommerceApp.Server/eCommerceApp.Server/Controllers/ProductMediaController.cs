using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerceApp.Contract;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.RequestFeatures;
using eCommerceApp.Server.ActionFilters;
using eCommerceApp.Server.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eCommerceApp.Server.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("/api/v{v:apiversion}/categories/{categoryId}/products/{productId}/media")]
    public class ProductMediaController : ControllerBase
    {
        private readonly IProductMediaService _productMediaService;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;

        public ProductMediaController(IProductMediaService productMediaService, ILoggerManager loggerManager, IMapper mapper)
        {
            _productMediaService = productMediaService;
            _loggerManager = loggerManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of media for Product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="parameter"></param>
        /// <returns>The list of media</returns>
        /// <response code="200">Return the list of media</response>
        /// <response code="404">If doesn't have any media of Product</response>
        [HttpGet(Name = "GetProductMedium")]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> GetProductMedium(Guid categoryId, Guid productId, [FromQuery] ProductMediaParameter parameter)
        {
            var productMediumInfo = await _productMediaService.GetMediumForProductAsync(productId,
                                                                              parameter.Type,
                                                                              trackChanges: false);
            var fileConvertedResult = new List<string>();

            if (productMediumInfo is not null)
            {
                foreach (var productMediaInfo in productMediumInfo)
                {
                    var fileResult = FileExtension.ConvertFileToBase64String(productMediaInfo, parameter.Type);
                    fileConvertedResult.Add(fileResult);
                }
            }

            if (fileConvertedResult is not null) return Ok(fileConvertedResult);
            else return NotFound();
        }

        /// <summary>
        /// Get a media for Product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="mediaId"></param>
        /// <param name="parameter"></param>
        /// <returns>A media for product</returns>
        /// <response code="404">If doesn't have any media of Product</response>
        /// <response code="200">Return a media</response>
        [HttpGet("{mediaId}", Name = "GetProductMedia")]
        public async Task<IActionResult> GetProductMedia(Guid categoryId, Guid productId, Guid mediaId, [FromQuery] ProductMediaParameter parameter)
        {
            var productMediaInfo = await _productMediaService.GetProductMediaAsync(productId, mediaId, false);

            if (productMediaInfo is not null)
            {
                var fileResult = FileExtension.ConvertFileToBase64String(productMediaInfo, parameter.Type);

                return Ok(JsonConvert.SerializeObject(fileResult));
            }

            return NotFound();
        }

        /// <summary>
        /// Upload a media for product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="parameter"></param>
        /// <param name="files"></param>
        /// <returns>Total file uploaded and sum of sizes</returns>
        /// <response code="200">If upload file successfully</response>
        /// <response code="400">If file is NULL or length = 0 or parameter is NULL</response>
        /// <response code="404">If CategoryId or ProductId does not exist in the database or them does not associated</response>
        [HttpPost]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<ActionResult> UploadProductMedium(Guid categoryId, Guid productId, [FromQuery] ProductMediaParameter parameter, List<IFormFile> files)
        {
            if (files is null || parameter is null)
            {
                return BadRequest("File is NULL or parameter is NULL");
            }

            long size = files.Sum(x => x.Length);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = DateTime.UtcNow.ToLocalTime().ToString("yyyyMMddHHmmssffff");
                    var fileExtension = Path.GetExtension(file.FileName).Substring(1);
                    var fileNameToSave = String.Join(".", fileName, fileExtension);

                    var mediaInformationToDB = new ProductMedia()
                    {
                        FileName = fileName,
                        FileExtension = fileExtension,
                        ProductId = productId,
                        Type = parameter.Type
                    };

                    _productMediaService.CreateProductMedia(mediaInformationToDB);

                    await file.SaveFileAsync(parameter.Type, fileNameToSave);
                }
            }

            await _productMediaService.SaveAsync();

            return Ok(new { count = files.Count, size });
        }

        /// <summary>
        /// Delete a product media for product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        /// <param name="mediaId"></param>
        /// <param name="parameter"></param>
        /// <returns>No content</returns>
        /// <response code="204">If delete successfully</response>
        /// <response code="404">If media with Id or file not found</response>
        [HttpDelete]
        [ServiceFilter(typeof(ValidateProductCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteProductMedia(Guid categoryId, Guid productId, Guid mediaId, [FromQuery] ProductMediaParameter parameter)
        {
            var productMedia = await _productMediaService.GetProductMediaAsync(productId, mediaId, false);
            if (productMedia is null)
            {
                return NotFound($"Doesn't exist media with id: {mediaId} in the database");
            }
            var path = FileExtension.GetTruePath(parameter.Type);
            var fullFileName = String.Join(".", productMedia.FileName, productMedia.FileExtension);
            var fullPath = Path.Combine(path, fullFileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);

                _productMediaService.DeleteProductMedia(productMedia);
                await _productMediaService.SaveAsync();

                return NoContent();
            }
            else
            {
                return NotFound("File not found in the media server");
            }
        }
    }
}