using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.Interfaces;
using pharmacy.infrastructuree.Data;

namespace pharmacy.web.Pages.Admin
{
    public class PharmaciesModel : PageModel
    {

        private readonly IPharmacyService _pharmacyService;

        public List<PharmacyAdminVM> Pharmacies { get; set; } = new();

        public PharmaciesModel(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        public async Task OnGetAsync()
        {
            var pharmacies = await _pharmacyService.GetAllPharmaciesAsync();

            Pharmacies = pharmacies.Select(p => new PharmacyAdminVM
            {
                Id = p.Id,
                Name = p.Name,
                OwnerName = "—",          // لو ضفت OwnerName للـ PharmacyDto بعدين حطه هنا
                Address = p.Address,
                Phone = p.Phone,
                Rating = p.Rating,
                IsActive = p.IsOpen,
                OrdersCount = 0           // لو ضفت OrdersCount للـ PharmacyDto بعدين حطه هنا
            }).ToList();
        }

        // ✅ Approve Handler
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            // هنا لما تضيف IsApproved للـ Entity بعدين
            // await _pharmacyService.ApprovePharmacyAsync(id);
            return RedirectToPage();
        }

        // ✅ Delete Handler
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // await _pharmacyService.DeletePharmacyAsync(id);
            return RedirectToPage();
        }

        // ─── View Model ───────────────────────────────────────────────
        public class PharmacyAdminVM
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string OwnerName { get; set; } = "";
            public string Address { get; set; } = "";
            public string Phone { get; set; } = "";
            public double Rating { get; set; }
            public bool IsActive { get; set; }
            public int OrdersCount { get; set; }
        }

    }
}
