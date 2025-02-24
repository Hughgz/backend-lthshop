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

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public PurchaseReceiptStatus Status { get; set; }
        public string PaymentType { get; set; }

        [Required]
        public string TransactionID { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
