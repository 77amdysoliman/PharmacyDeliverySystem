using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.Interfaces;

namespace pharmacy.web.Pages.Admin
{
    public class AddPharmacyModel : PageModel
    {
        private readonly IPharmacyService _pharmacyService;

        public AddPharmacyModel(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        [BindProperty]
        public AddPharmacyVM Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            
            await _pharmacyService.AddPharmacyAsync(
                Input.Name,
                Input.Address,
                Input.Phone
            );

            TempData["Success"] = "Pharmacy added successfully and is pending approval.";
            return RedirectToPage("/Admin/Pharmacies");
        }

        // ─── View Model ───────────────────────────────────────────────
        public class AddPharmacyVM
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Name is required")]
            public string Name { get; set; } = "";

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Address is required")]
            public string Address { get; set; } = "";

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Phone is required")]
            public string Phone { get; set; } = "";
        }
    }
}