using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class RevenueRepo : GenericRepo<Product>
    {
        public RevenueRepo(EcommerceDBContext context) : base(context)
        {
        }

        public async Task<List<Revenue>> GetRevenueWithPeriod(DateOnly fromDate, DateOnly toDate)
        {
            return await _context.Revenues
                .Where(r => new DateOnly(r.Year, r.Month, r.Day) >= fromDate &&
                            new DateOnly(r.Year, r.Month, r.Day) <= toDate)
                .ToListAsync();
        }

    }
}
