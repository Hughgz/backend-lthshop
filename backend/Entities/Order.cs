using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipping,
        Delivered,
        Cancelled,
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime DateTime { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentType { get; set; }
        public string TransactionID { get; set; }
        public int CustomerID { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}