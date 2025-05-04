using backend.Entities;

namespace backend.Repositories.EntitiesRepo
{
    public class PurchaseReceiptRepo : GenericRepo<PurchaseReceipt>
    {
        public PurchaseReceiptRepo(EcommerceDBContext context) : base(context) { }

        public async Task<PurchaseReceipt> GetPurchaseReceiptById(int id)
        {
            return await Task.FromResult(_context.Set<PurchaseReceipt>().Find(id));
        }

        public async Task<PurchaseReceipt> GetOneByFilter(Func<PurchaseReceipt, bool> predicate)
        {
            return await Task.FromResult(_context.Set<PurchaseReceipt>().FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<PurchaseReceipt>> GetManyByFilter(Func<PurchaseReceipt, bool> predicate)
        {
            return await Task.FromResult(_context.Set<PurchaseReceipt>().Where(predicate).ToList());
        }
    }
}
