using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class ProductPriceMapping : Profile
    {
        public ProductPriceMapping()
        {
            CreateMap<ProductPrice, ProductPriceReadDto>();
            CreateMap<ProductPriceCreateDto, ProductPrice>();
        }
    }
}
