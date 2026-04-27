using pharmacy.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.Interfaces
{
    public interface IPharmacyService
    {
        // بيجيب كل الصيدليات
        Task<IEnumerable<PharmacyDto>> GetAllPharmaciesAsync();

        // بيجيب صيدلية بالـ Id
        Task<PharmacyDto?> GetPharmacyByIdAsync(int id);

        // بيجيب أقرب صيدليات فيها دواء معين
        Task<IEnumerable<PharmacyDto>> GetNearestPharmaciesAsync(
            double latitude,
            double longitude,
            int medicineId);
    }
}
