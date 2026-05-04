using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.web.Pages.Helpers;

namespace pharamcy.web.Pages.Cart
{
    [Authorize]
    public class AddItemModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int PharmacyId { get; set; }

        [BindProperty]
        public string MedicineName { get; set; } = "";

        [BindProperty]
        public int Quantity { get; set; } = 1;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(MedicineName) && Quantity > 0)
            {
                CartHelper.AddItem(HttpContext.Session, new CartItem
                {
                    MedicineName = MedicineName,
                    Quantity = Quantity,
                    UnitPrice = 0 
                });
            }

            return RedirectToPage("/Cart/Index", new { PharmacyId });
        }
    }
}