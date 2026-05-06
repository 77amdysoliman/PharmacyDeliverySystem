using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pharmacy.Application.Sevices;
using pharmacy.domin.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pharmacy.web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public ForgotPasswordInput Input { get; set; } = new();

        public class ForgotPasswordInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // ✅ FIX: Base64Url encoding
                var tokenBytes = Encoding.UTF8.GetBytes(token);
                var encodedToken = WebEncoders.Base64UrlEncode(tokenBytes);

                var resetLink = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new
                    {
                        email = Input.Email,
                        token = encodedToken
                    },
                    protocol: Request.Scheme);

                await _emailService.SendPasswordResetEmailAsync(Input.Email, resetLink!);
            }

            TempData["EmailSent"] = "If this email exists, a reset link has been sent.";
            return Page();
        }
    }
}