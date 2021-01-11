using System.Threading.Tasks;

namespace eCommerceApp.Contract
{
    public interface IRepositoryManager
    {
        ICategoryRepository Category { get; }
        Task SaveAsync();
    }
}