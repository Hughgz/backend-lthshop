using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class ProductPriceRepo : GenericRepo<ProductPrice>
    {
        public ProductPriceRepo(EcommerceDBContext context) : base(context)
        {
        }

        public async Task<ProductPrice> GetActiveProductPriceByProductSizeId(int productSizeId)
        {
            return await _context.ProductPrice
                .Where(p => p.productPriceStatus == ProductPriceStatus.Active)
                .FirstOrDefaultAsync(p => p.ProductSizeId == productSizeId);
        }

        public async Task<IEnumerable<ProductPrice>> GetAllProductPriceByProductSizeId(int productSizeId)
        {
            return await _context.ProductPrice
                .Where(p => p.ProductSizeId == productSizeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductPrice>> GetAllProductPricePendingForApproval()
        {
            return await _context.ProductPrice
                .Where(p => p.productPriceStatus == ProductPriceStatus.PendingForApproval)
                .ToListAsync();
        }
    }
}
