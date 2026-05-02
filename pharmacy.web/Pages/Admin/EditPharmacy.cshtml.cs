using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using pharmacy.Application.DTOs;

namespace pharmacy.web.Pages.Admin
{
    public class EditPharmacyModel : PageModel
    {
        private readonly IPharmacyService _pharmacyService;

        public EditPharmacyModel(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        [BindProperty]
        public EditPharmacyVM Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pharmacy = await _pharmacyService.GetPharmacyByIdAsync(id);

            if (pharmacy == null)
                return NotFound();

            Input = new EditPharmacyVM
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                Address = pharmacy.Address,
                Phone = pharmacy.Phone,
                Rating = pharmacy.Rating,
                IsActive = pharmacy.IsOpen
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _pharmacyService.UpdatePharmacyAsync(
                    Input.Id,
                    Input.Name,
                    Input.Address,
                    Input.Phone,
                    Input.Rating,
                    Input.IsActive
                );

                TempData["Success"] = "Pharmacy updated successfully.";
                return RedirectToPage("/Admin/Pharmacies");
            }
            catch
            {
                TempData["Error"] = "Something went wrong. Please try again.";
                return Page();
            }
        }

        // ─── View Model ───────────────────────────────────────────────
        public class EditPharmacyVM
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; } = "";

            [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; } = "";

            [Required(ErrorMessage = "Phone is required")]
            public string Phone { get; set; } = "";

            [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
            public double Rating { get; set; }

            public bool IsActive { get; set; }
        }
    }
}