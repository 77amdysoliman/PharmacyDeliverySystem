using pharmacy.domin.Entites;
using pharmacy.domin.Interfaces;
using pharmacy.infrastructuree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.infrastructuree.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // Repositories
        public IGenericRepository<Pharmacy> Pharmacies { get; private set; }
        public IGenericRepository<Medicine> Medicines { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<PharmacyMedicine> PharmacyMedicines { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Order> Orders { get; private set; }
        public IGenericRepository<OrderItem> OrderItems { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            // Initialize Repositories
            Pharmacies = new GenericRepository<Pharmacy>(context);
            Medicines = new GenericRepository<Medicine>(context);
            Categories = new GenericRepository<Category>(context);
            PharmacyMedicines = new GenericRepository<PharmacyMedicine>(context);
            Users = new GenericRepository<User>(context);
            Orders = new GenericRepository<Order>(context);
            OrderItems = new GenericRepository<OrderItem>(context);
        }

        // Save All Changes
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
