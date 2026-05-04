using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Identity;

namespace Pharmacy.web.Pages.Location
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public double? Latitude { get; set; }

        [BindProperty]
        public double? Longitude { get; set; }

        [BindProperty]
        public string? Address { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.Latitude = Latitude;
                user.Longitude = Longitude;
                user.Address = Address;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToPage("/Index");
        }
    }
}