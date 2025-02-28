using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class PurchaseReceiptMapping : Profile
    {
        public PurchaseReceiptMapping()
        {
            CreateMap<PurchaseReceipt, PurchaseReceiptReadDto>();
            CreateMap<PurchaseReceiptCreateDto, PurchaseReceipt>();
        }
    }
}
