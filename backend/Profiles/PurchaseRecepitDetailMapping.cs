using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class PurchaseReceiptDetailMapping: Profile
    {
        public PurchaseReceiptDetailMapping()
        {
            CreateMap<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>();
            CreateMap<PurchaseReceiptDetailCreateDto, PurchaseReceiptDetail>();
            CreateMap<PurchaseReceiptDetailUpdateDto, PurchaseReceiptDetail>();
        }
    }
}
