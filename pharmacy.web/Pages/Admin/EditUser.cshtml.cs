using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace pharmacy.web.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditUserModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public EditUserVM Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return NotFound();

            // ✅ التعديل هنا (بدل orders)
            Input = new EditUserVM
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _unitOfWork.Users.GetByIdAsync(Input.Id);
            if (user == null) return NotFound();

            user.FullName = Input.FullName;
            user.Email = Input.Email;
            user.Phone = Input.Phone;

            // ✅ أهم سطر (الحل)
            user.IsActive = Input.IsActive;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            TempData["Success"] = "User updated successfully!";
            return RedirectToPage("./Users");
        }

        // ── ViewModel ───────────────────────────────────
        public class EditUserVM
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required")]
            public string FullName { get; set; } = "";

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "Phone is required")]
            public string Phone { get; set; } = "";

            public bool IsActive { get; set; }
        }
    }
}