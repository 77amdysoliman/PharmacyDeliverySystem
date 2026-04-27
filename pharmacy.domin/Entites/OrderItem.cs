namespace pharmacy.domin.Entites
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // FKs
        public int OrderId { get; set; }
        public int MedicineId { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Medicine Medicine { get; set; }
    }
}
