namespace backend.Dtos
{
    public class StockHistoryReadDto
    {
        public int StockHistoryID { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int ProductSizeID { get; set; }
        public int StockChange { get; set; }
        public string? Note { get; set; }
    }
}
