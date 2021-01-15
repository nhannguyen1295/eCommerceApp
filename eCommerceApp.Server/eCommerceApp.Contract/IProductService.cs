using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IProductService : IProductRepository
    {
        Task<IEnumerable<ProductCategory>> GetCategoriesForProductAsync(Guid productId, bool trackChanges);
        Task<IEnumerable<ProductCategory>> GetProductsForCategoryAsync(Guid categoryId, bool trackChanges);
        void DeleteProductCategory(Guid categoryId, Guid productId);
        Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId, bool trackChanges);
        Task CreateProductForCategory(Guid categoryId, Product product);
        void CreateProductCategory(Guid categoryId, Guid productId);
        void ChangeCategoryForProduct(Guid newCategoryId,
                                      ProductCategory productCategory);
        Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges);
        void CreateCategory(Category category);
        Task SaveAsync();
    }
}