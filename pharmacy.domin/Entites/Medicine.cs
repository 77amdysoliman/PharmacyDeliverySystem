namespace pharmacy.domin.Entites
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool RequiresPrescription { get; set; }
        public string? Manufacturer { get; set; }

        // FK
        public int CategoryId { get; set; }

        // Navigation
        public Category Category { get; set; }
        public ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
    }
}
