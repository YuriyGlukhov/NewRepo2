using ASP.Seminar1.Models;
using ASP.Seminar1.Models.DTO;
using AutoMapper;

namespace ASP.Seminar1.Repo
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductModel>(MemberList.Destination).ReverseMap();
            CreateMap<Category, CategoryModel>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageModel>(MemberList.Destination).ReverseMap();

        }

    }
}
