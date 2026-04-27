using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            return medicines.Select(m => new MedicineDto
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
    }
}