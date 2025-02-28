using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class PurchaseRecepitDetailMapping : Profile
    {
        public PurchaseRecepitDetailMapping()
        {
            CreateMap<PurchaseReceiptDetail, PurchaseReceiptDetailReadDto>();
            CreateMap<PurchaseReceiptDetailCreateDto, PurchaseReceiptDetail>();
            CreateMap<PurchaseReceiptDetailUpdateDto, PurchaseReceiptDetail>();
        }
    }
}
