using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class RevenueMapping : Profile
    {
        public RevenueMapping()
        {
            CreateMap<Revenue, RevenueReadDto>();
            CreateMap<RevenueCreateDto, Revenue>();
        }
    }
}
