using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pharmacy.web.Pages.Cart
{
    [Authorize]
    public class SavePrescriptionModel : PageModel
    {
        [BindProperty]
        public int PharmacyId { get; set; }

        [BindProperty]
        public int MedicineId { get; set; }

        [BindProperty]
        public string MedicineName { get; set; } = "";

        [BindProperty]
        public decimal UnitPrice { get; set; }

        [BindProperty]
        public string? PrescriptionBase64 { get; set; }

        [BindProperty]
        public string? PrescriptionFileName { get; set; }

        public IActionResult OnPost()
        {
            // ✅ احفظ الصورة في الـ Server Session
            if (!string.IsNullOrEmpty(PrescriptionBase64))
            {
                HttpContext.Session.SetString("PrescriptionImageBase64", PrescriptionBase64);
                HttpContext.Session.SetString("PrescriptionFileName", PrescriptionFileName ?? "prescription.jpg");
            }

            // روح على AddItem
            return RedirectToPage("/Cart/AddItem", new
            {
                PharmacyId,
                MedicineId,
                MedicineName,
                UnitPrice
            });
        }
    }
}