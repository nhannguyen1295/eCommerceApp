using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync(bool trackChanges);
        Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId, bool trackChanges);
        void CreateProductCategory(Guid categoryId, Guid productId);
        void DeleteProductCategory(Guid categoryId, Guid productId);
    }
}