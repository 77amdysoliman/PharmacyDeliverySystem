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
        public string DeliveryAddress { get; set; } = string.Empty;
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public string? PrescriptionImagePath { get; set; } 


        public string UserId { get; set; } = string.Empty;
        public int PharmacyId { get; set; }

        // Navigation
        public Pharmacy Pharmacy { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}