using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository.Extensions
{
    public static class SearchAndFilterExtension
    {
        public static async Task<IQueryable<T>> SearchAsync<T>(this IQueryable<T> entities, string searchTerm, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return entities;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            var entitiesToListAsync = await entities.ToListAsync<T>();

            return entitiesToListAsync.Where(x => x.GetType()
                                                   .GetProperty(propertyName)
                                                   .GetValue(x)
                                                   .ToString()
                                                   .ToLower()
                                                   .Contains(lowerCaseTerm)).AsQueryable<T>();
        }
    }
}