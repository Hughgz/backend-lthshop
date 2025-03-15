using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class PurchaseReceiptDetailRepo : GenericRepo<PurchaseReceiptDetail>
    {
        public PurchaseReceiptDetailRepo(EcommerceDBContext context) : base(context) { }

        public async Task<IEnumerable<PurchaseReceiptDetail>> GetManyByPurchaseReceiptId(Guid purchaseReceiptId)
        {
            return _context.PurchaseReceiptDetail.Where(p => p.PurchaseReceiptID == purchaseReceiptId).ToList();
        }
    }
}
