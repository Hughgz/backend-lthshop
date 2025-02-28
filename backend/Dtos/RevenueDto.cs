using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Dtos
{
    public class RevenueCreateDto
    {
        public DateOnly Date { get; set;}
        public decimal Amount { get; set; }
    }

    public class RevenueReadDto
    {
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
    }
}
