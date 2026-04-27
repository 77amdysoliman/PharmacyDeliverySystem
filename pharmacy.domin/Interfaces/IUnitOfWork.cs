namespace pharmacy.domin.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IGenericRepository<domin.Entites.Pharmacy> Pharmacies { get; }
        IGenericRepository<domin.Entites.Medicine> Medicines { get; }
        IGenericRepository<domin.Entites.Category> Categories { get; }
        IGenericRepository<domin.Entites.PharmacyMedicine> PharmacyMedicines { get; }
        IGenericRepository<domin.Entites.User> Users { get; }
        IGenericRepository<domin.Entites.Order> Orders { get; }
        IGenericRepository<domin.Entites.OrderItem> OrderItems { get; }

        // Save
        Task<int> CompleteAsync();
    }
}
            