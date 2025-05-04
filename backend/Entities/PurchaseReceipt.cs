using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public enum PurchaseReceiptStatus
    {
        Pending = 1,
        Processing = 2,
        Shipping = 3,
        Delivered = 4,
        Confirmed = 5,
        Cancelled = 6,
    }

    public class PurchaseReceipt
    {
        [Key]
        public int PurchaseReceiptID { get; set; }

        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public PurchaseReceiptStatus Status { get; set; }
        public string PaymentType { get; set; } 

        public string? TransactionID { get; set; }

        public int SupplierId { get; set; }

        // Navigation property
        public Supplier Supplier { get; set; }

        public ICollection<PurchaseReceiptDetail> PurchaseReceiptDetails { get; set; } = new List<PurchaseReceiptDetail>();
    }
}
