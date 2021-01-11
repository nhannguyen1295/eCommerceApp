using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;

namespace eCommerceApp.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryDataContext _repository;
        private ICategoryRepository _category;
        public RepositoryManager(RepositoryDataContext context) => _repository = context;

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null) _category = new CategoryRepository(_repository);
                return _category;
            }
        }

        public Task SaveAsync() => _repository.SaveChangesAsync();
    }
}