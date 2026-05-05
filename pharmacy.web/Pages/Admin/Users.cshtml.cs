using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.domin.Interfaces;

namespace pharmacy.web.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public List<UserVM> Users { get; set; } = new();

        public UsersModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var orders = await _unitOfWork.Orders.GetAllAsync();

            Users = users.Select((u, index) => new UserVM
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                OrdersCount = orders.Count(o => o.UserId == u.Id.ToString()),

                // ✅ مظبوطة
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Delete(user);
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToPage();
        }

        public class UserVM
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public int OrdersCount { get; set; }
            public bool IsActive { get; set; }
        }
    }
}