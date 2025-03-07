using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeID { get; set; }

        public int Size { get; set; }
        public double SalePrice { get; set; }
        public int StockQuantity { get; set; }

        public int RealQuantity { get; set; }

        public int ProductID { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
        public Product Product { get; set; }
    }
}

