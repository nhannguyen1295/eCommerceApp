using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;
using eCommerceApp.Repository;

namespace eCommerceApp.Service
{
    public class ProductService : ProductRepository, IProductService
    {
        private readonly IRepositoryManager _repositoryManager;
        public ProductService(RepositoryDataContext context,
                              IRepositoryManager repositoryManager) : base(context)
        {
            _repositoryManager = repositoryManager;
        }

        public Task SaveAsync() => _repositoryManager.SaveAsync();

        public async Task CreateProductForCategoryAsync(Guid categoryId, Product product)
        {
            _repositoryManager.Product.CreateProduct(product);
            await SaveAsync();
            _repositoryManager.ProductCategory.CreateProductCategory(categoryId, product.Id);
            await SaveAsync();
        }

        public void CreateProductCategory(Guid categoryId, Guid productId)
        => _repositoryManager.ProductCategory.CreateProductCategory(categoryId, productId);

        public async Task<IEnumerable<ProductCategory>> GetProductsForCategoryAsync(Guid categoryId, bool trackChanges)
        {
            var productCategoryList = await _repositoryManager.ProductCategory.GetProductCategoriesAsync(trackChanges);
            return productCategoryList.Where(x => x.CategoryId == categoryId);
        }

        public async Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId, bool trackChanges)
        => await _repositoryManager.ProductCategory.GetProductCategoryAsync(categoryId, productId, trackChanges);

        public void ChangeCategoryForProduct(Guid newCategoryId,
                                             ProductCategory productCategory)
        => productCategory.CategoryId = newCategoryId;

        public void DeleteProductCategory(Guid categoryId, Guid productId)
        => _repositoryManager.ProductCategory.DeleteProductCategory(categoryId, productId);

        public async Task<IEnumerable<ProductCategory>> GetCategoriesForProductAsync(Guid productId, bool trackChanges)
        {
            var productCategory = await _repositoryManager.ProductCategory.GetProductCategoriesAsync(trackChanges);
            return productCategory.Where(x => x.ProductId == productId);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges)
        => await _repositoryManager.Category.GetCategoriesAsync(trackChanges);

        public async Task CreateCategoryForProductAsync(Guid productId, Category category)
        {
            _repositoryManager.Category.CreateCategory(category);
            await SaveAsync();
            System.Console.WriteLine(category.Id.ToString(), Console.BackgroundColor = ConsoleColor.Blue);
            _repositoryManager.ProductCategory.CreateProductCategory(category.Id, productId);
            await SaveAsync();
        }
    }
}