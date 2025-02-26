using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class GoodsInspectionItems
    {
        [Key]
        public int Id { get; set; }
        public int GoodsInspectionId { get; set; }
        public int ProductId { get; set; }
        public int RealQuantity { get; set; }
    }
}
