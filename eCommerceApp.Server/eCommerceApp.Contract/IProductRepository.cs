using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.RequestFeatures;

namespace eCommerceApp.Contract
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProductsAsync(ProductParameters productParameters, bool trackChanges);
        Task<Product> GetProductAsync(Guid productId, bool trackChanges);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
    }
}