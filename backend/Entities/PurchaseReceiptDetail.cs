using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class PurchaseReceiptDetail
    {
        [Key]
        public int PurchaseReceiptDetailID { get; set; }

        public int PurchaseReceiptID { get; set; }

        public int ProductSizeID { get; set; }

        public string Unit { get; set; }

        public int Quantity { get; set; }

        public int? RealQuantity { get; set; }

        public decimal RawPrice { get; set; }

        // Navigation properties
        [JsonIgnore]
        public PurchaseReceipt PurchaseReceipt { get; set; }

        [JsonIgnore]
        public ProductSize ProductSize { get; set; }
    }
}
