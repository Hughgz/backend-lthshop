using backend.Entities;

namespace backend.Dtos
{
    public class ProductPriceReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProductSizeId { get; set; }
        public double SellingPrice { get; set; }
        public ProductPriceStatus productPriceStatus { get; set; }
        public string? Description { get; set; }
    }

    public class ProductPriceCreateDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProductSizeId { get; set; }
        public double SellingPrice { get; set; }
        public string? Description { get; set; }
    }
}
