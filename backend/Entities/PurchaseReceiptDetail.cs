using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Entities
{
    public class PurchaseReceiptDetail
    {
        [Key]
        public int PurchaseReceiptDetailID { get; set; }
        
        [Required]
        public int PurchaseReceiptID { get; set; }
        [JsonIgnore]
        public PurchaseReceipt PurchaseReceipt { get; set; }

        [Required]
        public int ProductSizeID { get; set; }
        [JsonIgnore]
        public ProductSize ProductSize { get; set; }

        [Required]
        public int Quantity { get; set; }

        public double Total { get; set; }
    }
}
