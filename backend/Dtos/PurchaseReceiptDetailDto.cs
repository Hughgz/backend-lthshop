using backend.Entities;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class PurchaseReceiptDetailReadDto
    {
        public int PurchaseReceiptDetailID { get; set; }
        public int PurchaseReceiptID { get; set; }
        public int ProductSizeID { get; set; }
        public string Unit { get; set;}
        public int Quantity { get; set; }
        public int? RealQuantity { get; set; }
        public decimal RawPrice { get; set; }

    }

    public class PurchaseReceiptDetailCreateDto
    {
        public int PurchaseReceiptID { get; set; }
        public int ProductSizeID { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal RawPrice { get; set; }
    }

    public class PurchaseReceiptDetailUpdateDto
    {
        public int PurchaseReceiptDetailID { get; set; }
        public int PurchaseReceiptID { get; set; }
        public int ProductSizeID { get; set; }
        public int? RealQuantity { get; set; }
    }
}
