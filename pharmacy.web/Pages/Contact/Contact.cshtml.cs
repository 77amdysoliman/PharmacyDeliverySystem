using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace pharmacy.web.Pages.Contact
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactInputModel Input { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // هنا تقدر تبعت Email أو تخزن في DB
            // SendEmail(Input);

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToPage();
        }
    }

    public class ContactInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^01[0125][0-9]{8}$",
            ErrorMessage = "Invalid Egyptian phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [MinLength(5, ErrorMessage = "Subject must be at least 5 characters")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}