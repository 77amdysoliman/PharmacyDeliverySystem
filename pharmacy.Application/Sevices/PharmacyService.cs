using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Entites;
using pharmacy.domin.Interfaces;

namespace pharmacy.Application.Sevices
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PharmacyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        // ✅ ADD
        public async Task AddPharmacyAsync(string name, string address, string phone)
        {
            var newPharmacy = new Pharmacy { Name = name, Address = address, Phone = phone, IsOpen = false };
            await _unitOfWork.Pharmacies.AddAsync(newPharmacy);
            await _unitOfWork.CompleteAsync();
        }

        // ✅ UPDATE
        public async Task UpdatePharmacyAsync(
            int id,
            string name,
            string address,
            string phone,
            double rating,
            bool isOpen)
        {
            var pharmacy = await _unitOfWork.Pharmacies.GetByIdAsync(id);

            if (pharmacy == null)
                throw new Exception("Pharmacy not found");

            pharmacy.Name = name;
            pharmacy.Address = address;
            pharmacy.Phone = phone;
            pharmacy.Rating = rating;
            pharmacy.IsOpen = isOpen;

            _unitOfWork.Pharmacies.Update(pharmacy);
            await _unitOfWork.CompleteAsync();
        }

        // ✅ APPROVE
        public async Task ApprovePharmacyAsync(int id)
        {
            var pharmacy = await _unitOfWork.Pharmacies.GetByIdAsync(id);

            if (pharmacy == null)
                return;

            pharmacy.IsOpen = true;

            _unitOfWork.Pharmacies.Update(pharmacy);
            await _unitOfWork.CompleteAsync();
        }

        // ✅ DELETE
        public async Task DeletePharmacyAsync(int id)
        {
            var pharmacy = await _unitOfWork.Pharmacies.GetByIdAsync(id);

            if (pharmacy == null)
                return;

            _unitOfWork.Pharmacies.Delete(pharmacy);
            await _unitOfWork.CompleteAsync();
        }

        // NEAREST
        public async Task<IEnumerable<PharmacyDto>> GetNearestPharmaciesAsync(
            double latitude,
            double longitude,
            int medicineId)
        {
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();

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
                    Distance = CalculateDistance(latitude, longitude, p.Latitude, p.Longitude)
                })
                .OrderBy(p => p.Distance)
                .ToList();
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return Math.Round(R * c, 2);
        }

        private double ToRad(double deg) => deg * Math.PI / 180;
    }
}