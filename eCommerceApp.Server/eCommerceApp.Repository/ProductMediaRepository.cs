using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository
{
    public class ProductMediaRepository : RepositoryBase<ProductMedia>, IProductMediaRepository
    {
        public ProductMediaRepository(RepositoryDataContext repository) : base(repository)
        {
        }

        public void CreateProductDetail(ProductMedia productMedia) => Create(productMedia);
        public void DeleteProductDetail(ProductMedia productMedia) => Delete(productMedia);

        public async Task<IEnumerable<ProductMedia>> GetProductMediasAsync(Guid productId, bool trackChanges)
        => await FindByCondition(x => x.ProductId == productId, trackChanges).ToListAsync();
    }
}