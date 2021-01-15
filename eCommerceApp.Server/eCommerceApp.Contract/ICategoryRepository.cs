using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.RequestFeatures;

namespace eCommerceApp.Contract
{
    public interface ICategoryRepository
    {
        Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges);
        Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges);
        Task<Category> GetCategoryAsync(Guid categoryId, bool trackChanges);
        void CreateCategory(Category category);
        Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCategory(Category category);
    }
}