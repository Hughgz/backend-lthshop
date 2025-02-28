namespace backend.Entities
{
    public class StockHistory
    {
        public int StockHistoryID { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int ProductID { get; set; }
        public int StockChange { get; set; }

        /// <summary>
        /// Note for the stock change (OrderID, GoodsInspectionID, etc.)
        /// </summary>
        public string? Note { get; set; }
        public Product Product { get; set; }
    }
}
