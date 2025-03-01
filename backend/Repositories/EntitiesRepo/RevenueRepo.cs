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

        public async Task<bool> CheckExistDate(DateTime date)
        {
            var checkDate = DateOnly.FromDateTime(date);
            var revenue = await _context.Revenues
                .AnyAsync(r => r.Date == checkDate); 

            return revenue;
        }

        public async Task<Revenue> GetByDate(DateTime date)
        {
            var dateOnly = DateOnly.FromDateTime(date);
            return await _context.Revenues.Where(r => r.Date == dateOnly).FirstOrDefaultAsync();

        }


    }
}
