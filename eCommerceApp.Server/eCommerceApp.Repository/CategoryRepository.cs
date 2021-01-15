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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryDataContext context) : base(context) { }

        public void CreateCategory(Category category) => Create(category);

        public void DeleteCategory(Category category) => Delete(category);

        public async Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters, bool trackChanges)
        {
            var categories = await FindAll(trackChanges).SearchAsync<Category>(categoryParameters.SearchTerm,
                                                                               "Name");
            var categoriesOrdered = categories.OrderBy(x => x.Name);
            return PagedList<Category>.ToPagedList(categoriesOrdered,
                                                   categoryParameters.PageNumber,
                                                   categoryParameters.PageSize);
        }

        public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public async Task<Category> GetCategoryAsync(Guid categoryId, bool trackChanges)
        => await FindByCondition(x => x.Id.Equals(categoryId), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool trackChanges)
        => await FindAll(trackChanges).ToListAsync();
    }
}