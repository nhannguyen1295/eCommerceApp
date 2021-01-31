using System.Threading.Tasks;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryDataContext _repository;
        private ICategoryRepository _category;
        private IProductRepository _product;
        private IProductCategoryRepository _productCategory;
        private IProductMediaRepository _productMedia;

        public RepositoryManager(RepositoryDataContext context) => _repository = context;

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null) _category = new CategoryRepository(_repository);
                return _category;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null) _product = new ProductRepository(_repository);
                return _product;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_productCategory == null) _productCategory = new ProductCategoryRepository(_repository);
                return _productCategory;
            }
        }


        public IProductMediaRepository ProductMedia
        {
            get
            {
                if (_productMedia is null) _productMedia = new ProductMediaRepository(_repository);
                return _productMedia;
            }
        }


        public Task SaveAsync() => _repository.SaveChangesAsync();
    }
}