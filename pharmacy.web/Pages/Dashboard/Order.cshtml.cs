using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Identity;

namespace Pharmacy.web.Pages.Dashboard
{
    public class OrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

        [TempData]
        public string? SuccessMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 10;

        public OrderModel(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<OrderDto> allOrders;

            if (User.IsInRole("SuperAdmin"))
                allOrders = await _orderService.GetAllOrdersAsync();
            else if (User.IsInRole("PharmacyAdmin") && user?.PharmacyId != null)
                allOrders = await _orderService.GetOrdersByPharmacyAsync(user.PharmacyId.Value);
            else
                allOrders = new List<OrderDto>();

            // Filter
            if (!string.IsNullOrEmpty(StatusFilter) && StatusFilter != "all")
                allOrders = allOrders.Where(o => o.Status == StatusFilter);

            // Pagination
            TotalCount = allOrders.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages == 0 ? 1 : TotalPages));

            Orders = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);
        }

        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Confirmed");
            TempData["SuccessMessage"] = "Order accepted! ✅";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Cancelled");
            TempData["SuccessMessage"] = "Order rejected! ❌";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeliverAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Delivered");
            TempData["SuccessMessage"] = "Order delivered! 🚀";
            return RedirectToPage();
        }
    }
}