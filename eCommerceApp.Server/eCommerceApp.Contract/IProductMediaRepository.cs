using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IProductMediaRepository
    {
        Task<IEnumerable<ProductMedia>> GetProductMediumAsync(Guid productId, bool trackChanges);
        Task<ProductMedia> GetProductMediaAsync(Guid productId, Guid mediaId, bool trackChanges);
        void CreateProductMedia(ProductMedia productMedia);
        void DeleteProductMedia(ProductMedia productMedia);
    }
}