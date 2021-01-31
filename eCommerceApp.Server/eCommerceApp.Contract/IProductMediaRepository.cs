using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IProductMediaRepository
    {
        Task<IEnumerable<ProductMedia>> GetProductMediasAsync(Guid productId, bool trackChanges);
        void CreateProductDetail(ProductMedia productMedia);
        void DeleteProductDetail(ProductMedia productMedia);
    }
}