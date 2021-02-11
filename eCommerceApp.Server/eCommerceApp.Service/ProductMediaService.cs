using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;
using eCommerceApp.Repository;

namespace eCommerceApp.Service
{
    public class ProductMediaService : ProductMediaRepository, IProductMediaService
    {
        private readonly IRepositoryManager _repositoryManager;
        public ProductMediaService(RepositoryDataContext repository, IRepositoryManager repositoryManager) : base(repository)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<ProductMedia>> GetMediumForProductAsync(Guid productId,
                                                                              MediaType type,
                                                                              bool trackChanges)
        {
            var medium = await _repositoryManager.ProductMedia.GetProductMediumAsync(productId, trackChanges);
            return medium.Where(x => x.Type.Equals(type));

        }

        public Task SaveAsync() => _repositoryManager.SaveAsync();
    }
}