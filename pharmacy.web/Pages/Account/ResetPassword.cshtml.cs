using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pharmacy.domin.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public ResetPasswordInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class ResetPasswordInput
        {
            public string Email { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;

            [Required]
            [MinLength(6)]
            public string NewPassword { get; set; } = string.Empty;

            [Required]
            [Compare("NewPassword")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string email, string token)
        {
            Input.Email = email ?? "";

            // ✅ FIX: Base64Url decode
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            Input.Token = Encoding.UTF8.GetString(decodedBytes);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                ErrorMessage = "Invalid request.";
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                Input.Token,
                Input.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password reset successfully.";
                return RedirectToPage("/Account/Login");
            }

            ErrorMessage = string.Join(" | ",
                result.Errors.Select(e => e.Description));

            return Page();
        }
    }
}