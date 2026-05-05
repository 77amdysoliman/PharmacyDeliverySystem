using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Identity;
using pharmacy.domin.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace pharmacy.web.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public List<UserVM> Users { get; set; } = new();

        public UsersModel(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var allUsers = await _userManager.Users.ToListAsync();

            var normalUsers = new List<ApplicationUser>();
            foreach (var u in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains("User") || roles.Count == 0)
                    normalUsers.Add(u);
            }

            Users = normalUsers.Select(u => new UserVM
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email ?? "",
                Phone = u.PhoneNumber ?? "",
                OrdersCount = orders.Count(o => o.UserId == u.Id),

                // ✅ Active لو عمل أوردر واحد على الأقل
                IsActive = orders.Any(o => o.UserId == u.Id),

                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return RedirectToPage();
        }

        public class UserVM
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public int OrdersCount { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}