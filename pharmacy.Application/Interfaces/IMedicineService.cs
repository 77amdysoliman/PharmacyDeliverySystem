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
        // بيجيب كل الأدوية
        Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync();

        // بيجيب دواء بالـ Id
        Task<MedicineDto?> GetMedicineByIdAsync(int id);

        // بيبحث عن دواء بالاسم
        Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(string name);

        // بيجيب أدوية Category معينة
        Task<IEnumerable<MedicineDto>> GetMedicinesByCategoryAsync(int categoryId);
    }
}
