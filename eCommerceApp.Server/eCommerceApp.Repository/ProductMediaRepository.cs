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

        public void CreateProductMedia(ProductMedia productMedia) => Create(productMedia);
        public void DeleteProductMedia(ProductMedia productMedia) => Delete(productMedia);

        public async Task<ProductMedia> GetProductMediaAsync(Guid productId, Guid mediaId, bool trackChanges)
        => await FindByCondition(x => x.ProductId == productId && x.Id == mediaId, trackChanges).FirstOrDefaultAsync();

        public async Task<IEnumerable<ProductMedia>> GetProductMediumAsync(Guid productId, bool trackChanges)
        => await FindByCondition(x => x.ProductId == productId, trackChanges).ToListAsync();
    }
}