namespace pharmacy.domin.Entites
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; }
        public bool IsOpen { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation
        public ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
