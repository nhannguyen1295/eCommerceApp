using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Repository;

namespace eCommerceApp.Service
{
    public class CategoryService : CategoryRepository, ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CategoryService(RepositoryDataContext context,
                               IRepositoryManager repositoryManager) : base(context)
        {
            _repositoryManager = repositoryManager;
        }

        public Task SaveAsync() => _repositoryManager.SaveAsync();
    }
}