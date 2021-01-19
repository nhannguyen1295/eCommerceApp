using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository.Extensions
{
    public static class SearchAndFilterExtension
    {
        /// <summary>
        /// Generic search
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="searchTerm"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Generic sort
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="orderByQueryString"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> entities, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return entities;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyFromQueryName,
                                                                                   StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null) continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return entities.OrderBy<T>(orderQuery.Split(" ")[0], false)
                               .AsQueryable<T>();
            }
            var property = orderQuery.Split(" ")[0];
            var typeOrder = orderQuery.Split(" ")[1];
            if (typeOrder == "ascending")
                return entities.OrderBy<T>(property, false);
            else return entities.OrderBy<T>(property, true);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key, support for Reflection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ordering"></param>
        /// <param name="descending"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool descending)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), (descending ? "OrderByDescending" : "OrderBy"),
                    new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(resultExp);
        }
    }
}