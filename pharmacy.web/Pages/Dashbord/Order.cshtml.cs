using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;

namespace pharmacy.web.Pages.Dashbord
{
    public class OrderModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

        [TempData]
        public string? SuccessMessage { get; set; }

        public OrderModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task OnGetAsync()
        {
            Orders = await _orderService.GetAllOrdersAsync();
        }

        // Accept Order
        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Confirmed");
            TempData["SuccessMessage"] = "Order accepted successfully! ✅";
            return RedirectToPage();
        }

        // Reject Order
        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Cancelled");
            TempData["SuccessMessage"] = "Order rejected! ❌";
            return RedirectToPage();
        }

        // Mark as Delivered
        public async Task<IActionResult> OnPostDeliverAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Delivered");
            TempData["SuccessMessage"] = "Order marked as delivered! 🚀";
            return RedirectToPage();
        }
    }
}