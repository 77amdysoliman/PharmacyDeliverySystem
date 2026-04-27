namespace pharmacy.domin.Entites
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }          
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        // Navigation
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}

