using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class WishlistedItem 
    {
        [Key]
        public int WishlistedItemID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
        public Product Product { get; set; }

    }
}
