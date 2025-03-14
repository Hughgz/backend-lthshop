namespace backend.Dtos
{
    public class WishlistedItemReadDto
    {
        public int WishlistedItemID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
    }

    public class WishlistedItemCreateDto
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
    }
}
