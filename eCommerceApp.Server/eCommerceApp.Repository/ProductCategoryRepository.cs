using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(RepositoryDataContext repository) : base(repository)
        {
        }

        public void CreateProductCategory(Guid categoryId, Guid productId)
        {
            var productCategory = new ProductCategory { CategoryId = categoryId, ProductId = productId };
            Create(productCategory);
        }

        public void DeleteProductCategory(Guid categoryId, Guid productId)
        {
            var productCategory = repository.ProductCategories.Where(x => x.CategoryId == categoryId && x.ProductId == productId).FirstOrDefault();
            if (productCategory is not null) Delete(productCategory);
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync(bool trackChanges)
        => await FindAll(trackChanges).ToListAsync();

        public async Task<ProductCategory> GetProductCategoryAsync(Guid categoryId, Guid productId, bool trackChanges)
        => await FindByCondition(x => x.CategoryId == categoryId && x.ProductId == productId, trackChanges).SingleOrDefaultAsync();
    }
}