namespace backend.Entities
{
    public class GoodsInspections
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CreatedById { get; set; }
        public int InchargePersonId { get; set; }

        public User CreatedByUser {  get; set; }
        public User InchargePerson { get; set; }

    }
}
