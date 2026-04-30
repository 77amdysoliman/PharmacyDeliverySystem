using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Interfaces;
using pharmacy.domin.Entites;

namespace pharmacy.Application.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync()
        {
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var categories = await _unitOfWork.Categories.GetAllAsync(); // ✅ ضيف السطر ده

            return medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                RequiresPrescription = m.RequiresPrescription,
                Manufacturer = m.Manufacturer,
                CategoryName = categories.FirstOrDefault(c => c.Id == m.CategoryId)?.Name ?? "",
                IsAvailable = pharmacyMedicines
                    .Any(pm => pm.MedicineId == m.Id && pm.IsAvailable),
                Stock = pharmacyMedicines
                    .Where(pm => pm.MedicineId == m.Id)
                    .Sum(pm => pm.Stock)
            });
        }

        public async Task<MedicineDto?> GetMedicineByIdAsync(int id)
        {
            var m = await _unitOfWork.Medicines.GetByIdAsync(id);
            if (m == null) return null;
            return new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                RequiresPrescription = m.RequiresPrescription,
                Manufacturer = m.Manufacturer,
            };
        }

        public async Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(string name)
        {
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            return medicines
                .Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(m => new MedicineDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    RequiresPrescription = m.RequiresPrescription,
                    Manufacturer = m.Manufacturer,
                });
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesByCategoryAsync(int categoryId)
        {
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            return medicines
                .Where(m => m.CategoryId == categoryId)
                .Select(m => new MedicineDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    RequiresPrescription = m.RequiresPrescription,
                    Manufacturer = m.Manufacturer,
                });
        }
        public async Task CreateMedicineAsync(MedicineDto dto)
        {
            // 1️⃣ ضيف الدواء في Medicines
            var medicine = new Medicine
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                RequiresPrescription = dto.RequiresPrescription,
                Manufacturer = dto.Manufacturer,
                CategoryId = dto.CategoryId
            };
            await _unitOfWork.Medicines.AddAsync(medicine);
            await _unitOfWork.CompleteAsync();

            // 2️⃣ ضيفه تلقائي في PharmacyMedicines
            var pharmacyMedicine = new PharmacyMedicine
            {
                MedicineId = medicine.Id,
                PharmacyId = 1,
                Stock = dto.Stock,
                Price = dto.Price,
                IsAvailable = dto.Stock > 0
            };
            await _unitOfWork.PharmacyMedicines.AddAsync(pharmacyMedicine);
            await _unitOfWork.CompleteAsync();
        }

        // UPDATE
        public async Task UpdateMedicineAsync(MedicineDto dto)
        {
            // 1️⃣ عدل في Medicines
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(dto.Id);
            if (medicine == null) return;

            medicine.Name = dto.Name;
            medicine.Description = dto.Description;
            medicine.Price = dto.Price;
            medicine.ImageUrl = dto.ImageUrl;
            medicine.RequiresPrescription = dto.RequiresPrescription;
            medicine.Manufacturer = dto.Manufacturer;
            medicine.CategoryId = dto.CategoryId; 


            _unitOfWork.Medicines.Update(medicine);
            await _unitOfWork.CompleteAsync();

            // 2️⃣ عدل في PharmacyMedicines
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var pharmacyMedicine = pharmacyMedicines
                .FirstOrDefault(pm => pm.MedicineId == dto.Id);

            if (pharmacyMedicine != null)
            {
                pharmacyMedicine.Stock = dto.Stock;
                pharmacyMedicine.Price = dto.Price;
                pharmacyMedicine.IsAvailable = dto.Stock > 0;

                _unitOfWork.PharmacyMedicines.Update(pharmacyMedicine);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                // ✅ لو مش موجود اعمله جديد
                var newPharmacyMedicine = new PharmacyMedicine
                {
                    MedicineId = dto.Id,
                    PharmacyId = 1,
                    Stock = dto.Stock,
                    Price = dto.Price,
                    IsAvailable = dto.Stock > 0
                };
                await _unitOfWork.PharmacyMedicines.AddAsync(newPharmacyMedicine);
            }

                await _unitOfWork.CompleteAsync();
            }
        
        

        // DELETE
        public async Task DeleteMedicineAsync(int id)
        {
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(id);
            if (medicine == null) return;

            _unitOfWork.Medicines.Delete(medicine);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IconUrl = c.IconUrl
            });
        }
    }
}