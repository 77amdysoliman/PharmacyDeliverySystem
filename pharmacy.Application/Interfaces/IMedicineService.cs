using pharmacy.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync();
        Task<MedicineDto?> GetMedicineByIdAsync(int id);
        Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(string name);
        Task<IEnumerable<MedicineDto>> GetMedicinesByCategoryAsync(int categoryId);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task CreateMedicineAsync(MedicineDto dto);
        Task UpdateMedicineAsync(MedicineDto dto);
        Task DeleteMedicineAsync(int id);
    }
}
