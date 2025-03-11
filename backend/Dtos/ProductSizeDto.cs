namespace backend.Dtos
{
    public class ProductSizeReadDto
    {
        public int ProductSizeID { get; set; }
        public int Size { get; set; }
<<<<<<< HEAD
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
=======

        public int StockQuantity { get; set; }
        public int RealQuantity { get; set; }
>>>>>>> 4529aeecf008d171a1c154f2a4c69bfff1db8a84
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

    public class ProductSizeCreateDto
    {
        public int Size { get; set; }
<<<<<<< HEAD
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
=======
>>>>>>> 4529aeecf008d171a1c154f2a4c69bfff1db8a84
        public int ProductID { get; set; }
    }

    public class ProductSizeUpdateDto
    {
        public int Size { get; set; }
<<<<<<< HEAD
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; } // Đổi từ Quantity thành StockQuantity
        public int RealQuantity { get; set; } // Thêm RealQuantity
=======
        public int StockQuantity { get; set; }
        public int RealQuantity { get; set; }
>>>>>>> 4529aeecf008d171a1c154f2a4c69bfff1db8a84
        public int ProductID { get; set; }
    }
}
