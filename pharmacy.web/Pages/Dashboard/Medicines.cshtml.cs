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

        [BindProperty]
        public MedicineDto Medicine { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        public IndexModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public async Task OnGetAsync()
        {
            Medicines = string.IsNullOrEmpty(SearchTerm)
                ? await _medicineService.GetAllMedicinesAsync()
                : await _medicineService.SearchMedicinesAsync(SearchTerm);

            Categories = await _medicineService.GetAllCategoriesAsync();
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