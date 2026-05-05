using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Identity;

namespace Pharmacy.web.Pages.MyOrders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<OrderDto> Orders { get; set; } = new();

        public IndexModel(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                Orders = (await _orderService.GetOrdersByUserAsync(user.Id)).ToList();
        }
    }
}