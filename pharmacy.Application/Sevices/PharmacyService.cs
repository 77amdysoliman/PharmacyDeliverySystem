using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.Sevices
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PharmacyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // بيجيب كل الصيدليات
        public async Task<IEnumerable<PharmacyDto>> GetAllPharmaciesAsync()
        {
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();
            return pharmacies.Select(p => new PharmacyDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Rating = p.Rating,
                IsOpen = p.IsOpen,
                ImageUrl = p.ImageUrl,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
            });
        }

        // بيجيب صيدلية بالـ Id
        public async Task<PharmacyDto?> GetPharmacyByIdAsync(int id)
        {
            var p = await _unitOfWork.Pharmacies.GetByIdAsync(id);
            if (p == null) return null;
            return new PharmacyDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Rating = p.Rating,
                IsOpen = p.IsOpen,
                ImageUrl = p.ImageUrl,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
            };
        }

        // بيجيب أقرب صيدليات فيها دواء معين
        public async Task<IEnumerable<PharmacyDto>> GetNearestPharmaciesAsync(
            double latitude,
            double longitude,
            int medicineId)
        {
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();

            // بيجيب الصيدليات اللي فيها الدواء ده ومتاح
            var availablePharmacyIds = pharmacyMedicines
                .Where(pm => pm.MedicineId == medicineId && pm.IsAvailable && pm.Stock > 0)
                .Select(pm => pm.PharmacyId)
                .ToList();

            return pharmacies
                .Where(p => availablePharmacyIds.Contains(p.Id) && p.IsOpen)
                .Select(p => new PharmacyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    Phone = p.Phone,
                    Rating = p.Rating,
                    IsOpen = p.IsOpen,
                    ImageUrl = p.ImageUrl,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    // حساب المسافة بـ Haversine Formula
                    Distance = CalculateDistance(latitude, longitude, p.Latitude, p.Longitude)
                })
                .OrderBy(p => p.Distance)  // ترتيب من الأقرب للأبعد
                .ToList();
        }

        // Haversine Formula لحساب المسافة بين نقطتين
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // نصف قطر الأرض بالكيلومتر
            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return Math.Round(R * c, 2); // المسافة بالكيلومتر
        }

        private double ToRad(double deg) => deg * Math.PI / 180;
    }
}
