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

    public async Task<IEnumerable<StockHistory>> GetManyByProductSizeIdAndPeriod(int? productSizeId, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.StockHistories.AsQueryable();

        if (productSizeId.HasValue)
        {
            query = query.Where(sh => sh.ProductSizeID == productSizeId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(sh => sh.UpdatedDateTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(sh => sh.UpdatedDateTime <= endDate.Value);
        }

        return await query.ToListAsync();
    }



}
