namespace backend.Entities
{
    public enum DeliveryStatus
    {
        Pending,
        Delivered,
        Cancelled
    }

    public class DeliveryOrder
    {
        public int DeliveryOrderID { get; set; }
        public int OrderId { get ; set; }
        public DateTime CreatedAt { get; set; }
        public string? DeliverySupplier { get; set; }
        public string? DeliveryCode { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DeliveryStatus Status { get; set; }
        public string? DeliveryNote { get; set; }
        public Order Order { get; set; }
    }
}
