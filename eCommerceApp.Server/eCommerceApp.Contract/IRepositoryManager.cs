using System.Threading.Tasks;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Contract
{
    public interface IRepositoryManager
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductMediaRepository ProductMedia { get; }

        Task SaveAsync();
    }
}