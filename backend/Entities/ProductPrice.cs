using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace backend.Entities
{
    public enum ProductPriceStatus
    {
        Active,
        Inactive,
        PendingForApproval
    }

    public class ProductPrice
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProductSizeId { get; set; }
        public decimal SellingPrice { get; set; }
        public ProductPriceStatus productPriceStatus { get; set; }
        public string? Description { get; set; }
        public ProductSize ProductSize { get; set; }
    }
}
