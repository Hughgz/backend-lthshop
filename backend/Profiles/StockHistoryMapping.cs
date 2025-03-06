using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class StockHistoryMapping : Profile
    {
        public StockHistoryMapping()
        {
            CreateMap<StockHistory, StockHistoryReadDto>();
        }
    }
}
