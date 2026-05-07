using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;

namespace Pharmacy.web.Pages.Medicines
{
    public class IndexModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public IEnumerable<MedicineDto> Medicines { get; set; } = new List<MedicineDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PrescriptionFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? AvailabilityFilter { get; set; }

        [BindProperty]
        public MedicineDto Medicine { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public IndexModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public async Task OnGetAsync()
        {
            var allMedicines = string.IsNullOrEmpty(SearchTerm)
                ? await _medicineService.GetAllMedicinesAsync()
                : await _medicineService.SearchMedicinesAsync(SearchTerm);

            Categories = await _medicineService.GetAllCategoriesAsync();

            // Category Filter
            if (!string.IsNullOrEmpty(CategoryFilter) && CategoryFilter != "all")
                allMedicines = allMedicines.Where(m =>
                    m.CategoryName?.ToLower() == CategoryFilter.ToLower());

            // Prescription Filter
            if (PrescriptionFilter == "required")
                allMedicines = allMedicines.Where(m => m.RequiresPrescription);
            else if (PrescriptionFilter == "notrequired")
                allMedicines = allMedicines.Where(m => !m.RequiresPrescription);

            // Availability Filter
            if (AvailabilityFilter == "available")
                allMedicines = allMedicines.Where(m => m.IsAvailable && m.Stock > 0);
            else if (AvailabilityFilter == "outofstock")
                allMedicines = allMedicines.Where(m => !m.IsAvailable || m.Stock == 0);

            TotalCount = allMedicines.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages == 0 ? 1 : TotalPages));

            Medicines = allMedicines
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                Medicines = await _medicineService.GetAllMedicinesAsync();
                Categories = await _medicineService.GetAllCategoriesAsync();
                return Page();
            }
            await _medicineService.CreateMedicineAsync(Medicine);
            TempData["SuccessMessage"] = "Medicine added successfully! ✅";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                Medicines = await _medicineService.GetAllMedicinesAsync();
                Categories = await _medicineService.GetAllCategoriesAsync();
                return Page();
            }
            await _medicineService.UpdateMedicineAsync(Medicine);
            TempData["SuccessMessage"] = "Medicine updated successfully! ✅";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _medicineService.DeleteMedicineAsync(id);
            TempData["SuccessMessage"] = "Medicine deleted successfully! ✅";
            return RedirectToPage();
        }
    }
}