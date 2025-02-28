using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class RevenueRepo : GenericRepo<Revenue>
    {
        public RevenueRepo(EcommerceDBContext context) : base(context)
        {
        }

        public async Task<List<Revenue>> GetRevenueWithPeriod(DateOnly fromDate, DateOnly toDate)
        {
            return await _context.Revenues
                .Where(r => r.Date >= fromDate && r.Date <= toDate)
                .ToListAsync();
        }

    }
}
