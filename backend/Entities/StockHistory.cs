using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class StockHistory
    {
        [Key]
        public int StockHistoryID { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int ProductSizeID { get; set; }
        public int StockChange { get; set; }

        /// <summary>
        /// Note for the stock change (OrderID, GoodsInspectionID, etc.)
        /// </summary>
        public string? Note { get; set; }
        public ProductSize ProductSize { get; set; }
    }
}
