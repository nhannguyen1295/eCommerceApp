using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.RequestFeatures;
using eCommerceApp.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryDataContext context) : base(context) { }

        public void CreateProduct(Product product) => Create(product);

        public void DeleteProduct(Product product) => Delete(product);

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public async Task<Product> GetProductAsync(Guid productId, bool trackChanges)
        => await FindByCondition(x => x.Id.Equals(productId), trackChanges).SingleOrDefaultAsync();

        public async Task<PagedList<Product>> GetProductsAsync(ProductParameters productParameters, bool trackChanges)
        {
            var products = await FindAll(trackChanges).SearchAsync(productParameters.SearchTerm, "Name");
            var productsOrdered = products.OrderBy(x => x.Name);
            return PagedList<Product>.ToPagedList(productsOrdered,
                                                  productParameters.PageNumber,
                                                  productParameters.PageSize);
        }
    }
}