using backend.Entities;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class PurchaseReceiptDetailReadDto
    {
        public int PurchaseReceiptDetailID { get; set; }
        public int PurchaseReceiptID { get; set; }
        public int ProductSizeID { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }

    public class PurchaseReceiptDetailCreateDto
    {
        public int PurchaseReceiptID { get; set; }
        public int ProductSizeID { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
