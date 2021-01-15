using System.Threading.Tasks;

namespace eCommerceApp.Contract
{
    public interface IRepositoryManager
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        Task SaveAsync();
    }
}