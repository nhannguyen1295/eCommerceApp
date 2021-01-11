using System.Threading.Tasks;

namespace eCommerceApp.Contract
{
    public interface ICategoryService : ICategoryRepository
    {
        Task SaveAsync();
    }
}