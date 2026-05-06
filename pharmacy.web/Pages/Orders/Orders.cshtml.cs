using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Identity;
using pharmacy.domin.Interfaces;

namespace pharmacy.web.Pages.Orders
{


    public class OrdersModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMedicineService _medicineService;

        public List<MedicineDto> Medicines { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }

        // Pagination
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? AvailFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MedicineName { get; set; }

        public OrdersModel(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IMedicineService medicineService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _medicineService = medicineService;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                UserLatitude = user?.Latitude ?? 30.0444;
                UserLongitude = user?.Longitude ?? 31.2357;
            }

            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var pharmacyMedicines = await _unitOfWork.PharmacyMedicines.GetAllAsync();
            var categories = await _unitOfWork.Categories.GetAllAsync();

            Categories = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            var allMedicines = medicines.Select(m => new MedicineDto
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
            }).AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(SearchQuery))
                allMedicines = allMedicines.Where(m =>
                    m.Name.ToLower().Contains(SearchQuery.ToLower()) ||
                    (m.CategoryName != null && m.CategoryName.ToLower().Contains(SearchQuery.ToLower())));

            // Category Filter
            if (!string.IsNullOrWhiteSpace(CategoryFilter) && CategoryFilter != "all")
                allMedicines = allMedicines.Where(m =>
                    m.CategoryName != null && m.CategoryName.ToLower() == CategoryFilter.ToLower());

            // Availability Filter
            if (AvailFilter == "available")
                allMedicines = allMedicines.Where(m => m.IsAvailable);

            var filtered = allMedicines.ToList();

            // Pagination
            TotalCount = filtered.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages == 0 ? 1 : TotalPages));

            Medicines = filtered
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (!string.IsNullOrEmpty(MedicineName))
            {
                Medicines = Medicines
                    .Where(m => m.Name.Contains(MedicineName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}