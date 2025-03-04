using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo;

public class StockHistoryRepo : GenericRepo<StockHistory>
{
    public StockHistoryRepo(EcommerceDBContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StockHistory>> GetAllByProductSizeId(int productSizeId)
    {
        return await _context.StockHistories
            .Where(sh => sh.ProductSizeID == productSizeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<StockHistory>> GetManyByProductSizeIdAndPeriod(int productSizeId, DateTime startDate, DateTime endDate)
    {
        return await _context.StockHistories
            .Where(sh => sh.ProductSizeID == productSizeId && sh.UpdatedDateTime >= startDate && sh.UpdatedDateTime <= endDate)
            .ToListAsync();
    }


}
