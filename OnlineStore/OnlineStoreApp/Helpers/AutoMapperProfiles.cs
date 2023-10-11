using AutoMapper;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.PriceBtc, opt => opt.Ignore());      
        }
    }
}