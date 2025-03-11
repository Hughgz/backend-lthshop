using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class WishlistedItemRepo : GenericRepo<WishlistedItem>
    {
        public WishlistedItemRepo(EcommerceDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WishlistedItem>> GetWishlistedItemsByCustomerId(int customerId)
        {
            return await _context.WishlistedItems.Where(w => w.CustomerID == customerId).ToListAsync();
        }
    }
}
