using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public class LoginInputModel
        {
            [TempData]
            public string? SuccessMessage { get; set; }
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var roles = await _userManager.GetRolesAsync(user!);

                if (roles.Contains("SuperAdmin"))
                    return RedirectToPage("/Admin/Index");

                else if (roles.Contains("PharmacyAdmin"))
                {
                    // ✅ بيروح Dashboard بتاع صيدليته
                    return RedirectToPage("/Dashboard/Index", new { pharmacyId = user!.PharmacyId });
                }

                else
                    return RedirectToPage("/Location/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return Page();
        }
    }
}