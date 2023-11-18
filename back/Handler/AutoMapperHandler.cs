using AutoMapper;
using back.Models.Domain;
using back.Repositories;

namespace back.Handler
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<Product, ProductEntity>()
                .ForMember(item => item.Banner, opt => opt.MapFrom(item => item.Thumbnail)).ReverseMap();
        }
    }
}
