using AutoMapper;

namespace backend.Profiles
{
    public class WishlistedItemMapping : Profile
    {
        public WishlistedItemMapping()
        {
            CreateMap<Entities.WishlistedItem, Dtos.WishlistedItemReadDto>();
            CreateMap<Dtos.WishlistedItemCreateDto, Entities.WishlistedItem>();
        }
    }
}
