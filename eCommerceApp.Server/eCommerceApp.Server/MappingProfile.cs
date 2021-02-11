using AutoMapper;
using eCommerceApp.Entities.DTO;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.Models.Identity;

namespace eCommerceApp.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryForCreationDTO, Category>();
            CreateMap<CategoryForUpdateDTO, Category>();
            CreateMap<CategoryForUpdateDTO, Category>().ReverseMap();

            CreateMap<UserForRegistrationDTO, User>();
            CreateMap<UserForUpdateDTO, User>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductForCreationDTO, Product>();
            CreateMap<ProductForUpdateDTO, Product>();
            CreateMap<ProductForUpdateDTO, Product>().ReverseMap();
            CreateMap<CategoryForProductUpdateDTO, Category>();

            CreateMap<ProductMedia, ProductMediaDTO>().ForMember(x => x.File, opt => opt.Ignore());
        }
    }
}