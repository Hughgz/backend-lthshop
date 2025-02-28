using backend.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class PurchaseReceiptCreateDto
    {
        public DateTime DateTime { get; set; }

        public decimal TotalPrice { get; set; }

        public PurchaseReceiptStatus Status { get; set; }
        public string PaymentType { get; set; }

        public string TransactionID { get; set; }

        public int SupplierId { get; set; }

        public IEnumerable<PurchaseReceiptDetailCreateDto> Details { get; set; }
    }

    public class PurchaseReceiptReadDto
    {
        public Guid PurchaseReceiptID { get; set; }

        public DateTime DateTime { get; set; }

        public decimal TotalPrice { get; set; }

        public PurchaseReceiptStatus Status { get; set; }
        public string PaymentType { get; set; }

        public string TransactionID { get; set; }

        public int SupplierId { get; set; }

        public IEnumerable<PurchaseReceiptDetailReadDto> Details { get; set; }
    }
}
