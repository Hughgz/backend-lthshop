using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Dtos
{
    public class RevenueCreateDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal Amount { get; set; }
    }

    public class RevenueReadDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal Amount { get; set; }
    }
}
