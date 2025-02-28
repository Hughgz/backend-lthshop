using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class GoodsInspection
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CreatedById { get; set; }
        public int InchargePersonId { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedByUser {  get; set; }

        [ForeignKey("InchargePersonId")]
        public User InchargePerson { get; set; }

    }
}
