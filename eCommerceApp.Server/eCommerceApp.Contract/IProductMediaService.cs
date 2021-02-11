using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IProductMediaService : IProductMediaRepository
    {
        // Get all media
        Task<IEnumerable<ProductMedia>> GetMediumForProductAsync(Guid productId, MediaType type, bool trackChanges);
        Task SaveAsync();
    }
}