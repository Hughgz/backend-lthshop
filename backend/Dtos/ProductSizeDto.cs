namespace backend.Dtos
{
    public class ProductSizeReadDto
    {
        public int ProductSizeID { get; set; }
        public int Size { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductSizeCreateDto
    {
        public int Size { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
        public int ProductID { get; set; }
    }

    public class ProductSizeUpdateDto
    {
        public int Size { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
        public int ProductID { get; set; }
    }
}
