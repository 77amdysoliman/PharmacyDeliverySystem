using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Identity;
using System.ComponentModel.DataAnnotations;

namespace pharmacy.web.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserVM Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            Input = new EditUserVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Phone = user.PhoneNumber ?? ""
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null) return NotFound();

            user.FullName = Input.FullName;
            user.Email = Input.Email;
            user.PhoneNumber = Input.Phone;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return Page();
            }

            TempData["Success"] = "User updated successfully!";
            return RedirectToPage("./Users");
        }

        public class EditUserVM
        {
            public string Id { get; set; } = "";

            [Required(ErrorMessage = "Name is required")]
            public string FullName { get; set; } = "";

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Phone is required")]
            public string Phone { get; set; } = "";
        }
    }
}