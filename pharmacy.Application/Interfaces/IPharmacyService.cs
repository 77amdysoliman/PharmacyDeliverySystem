using pharmacy.Application.DTOs;

namespace pharmacy.Application.Interfaces
{
    public interface IPharmacyService
    {
        // GET ALL
        Task<IEnumerable<PharmacyDto>> GetAllPharmaciesAsync();

        // GET BY ID
        Task<PharmacyDto?> GetPharmacyByIdAsync(int id);

        // NEAREST
        Task<IEnumerable<PharmacyDto>> GetNearestPharmaciesAsync(
            double latitude,
            double longitude,
            int medicineId);

        // ✅ ADD THESE
        Task UpdatePharmacyAsync(int id, string name, string address, string phone, double rating, bool isOpen);

        Task ApprovePharmacyAsync(int id);

        Task DeletePharmacyAsync(int id);
    }
}