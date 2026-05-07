using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Identity;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.web.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; } = "";

        public class InputModel
        {
            [Required(ErrorMessage = "Password is required")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Confirm Password is required")]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var decodedToken = Uri.UnescapeDataString(Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, Input.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password reset successfully! ✅";
                return RedirectToPage("/Account/Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}