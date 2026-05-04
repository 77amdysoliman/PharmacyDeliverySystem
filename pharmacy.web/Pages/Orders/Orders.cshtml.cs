using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.domin.Interfaces;

namespace pharmacy.web.Pages.Orders
{
    public class OrdersModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public List<MedicineDto> Medicines { get; set; } = new();

        public OrdersModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync()
        {
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var categories = await _unitOfWork.Categories.GetAllAsync();

            Medicines = medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                CategoryName = categories.FirstOrDefault(c => c.Id == m.CategoryId)?.Name ?? "",
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = pharmacyMedicines.Any(pm => pm.MedicineId == m.Id && pm.IsAvailable),
                Stock = pharmacyMedicines.Where(pm => pm.MedicineId == m.Id).Sum(pm => pm.Stock),
                RequiresPrescription = m.RequiresPrescription,
                Manufacturer = m.Manufacturer
            }).ToList();
        }
    }
}