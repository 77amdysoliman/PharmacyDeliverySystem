namespace pharmacy.domin.Entites
{
    public class PharmacyMedicine
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public int MedicineId { get; set; }
        public int Stock { get; set; }          
        public decimal Price { get; set; }       
        public bool IsAvailable { get; set; }

        public Pharmacy Pharmacy { get; set; }
        public Medicine Medicine { get; set; }
    }
}
