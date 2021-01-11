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
        }
    }
}