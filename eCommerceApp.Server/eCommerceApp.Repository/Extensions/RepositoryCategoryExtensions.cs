using System.Linq;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Repository.Extensions
{
    public static class RepositoryCategoryExtensions
    {
        public static IQueryable<Category> Search(this IQueryable<Category> categories, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return categories;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return categories.Where(x => x.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}