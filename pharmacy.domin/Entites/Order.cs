namespace pharmacy.domin.Entites
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Preparing,
        OutForDelivery,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }
        public string DeliveryAddress { get; set; }

        // FKs
        public int UserId { get; set; }
        public int PharmacyId { get; set; }

        // Navigation
        public User user { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
}
