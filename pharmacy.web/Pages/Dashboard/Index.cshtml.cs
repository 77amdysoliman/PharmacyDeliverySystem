using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Identity;

namespace Pharmacy.web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMedicineService _medicineService;
        private readonly UserManager<ApplicationUser> _userManager;


        public int NewOrders { get; set; }
        public int PendingOrders { get; set; }
        public int AvailableMedicines { get; set; }
        public decimal TodaySales { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalOrders { get; set; }
        public int PageSize { get; set; } = 10;
        public IEnumerable<OrderDto> RecentOrders { get; set; } = new List<OrderDto>();

        public IndexModel(
           IOrderService orderService,
           IMedicineService medicineService,
           UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _medicineService = medicineService;
            _userManager = userManager; 
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<OrderDto> allOrders;

            if (User.IsInRole("PharmacyAdmin") && user?.PharmacyId != null)
                allOrders = await _orderService.GetOrdersByPharmacyAsync(user.PharmacyId.Value);
            else
                allOrders = await _orderService.GetAllOrdersAsync();

            var ordersList = allOrders.ToList();

            // ✅ الحسابات الصح
            NewOrders = ordersList.Count(o => o.Status == "Pending");
            PendingOrders = ordersList.Count(o => o.Status == "Confirmed" || o.Status == "Preparing");
            TodaySales = ordersList
                .Where(o => o.OrderDate.Date == DateTime.UtcNow.Date) // ✅ UtcNow مش Today
                .Sum(o => o.TotalPrice);

            // ✅ Pagination
            TotalOrders = ordersList.Count;
            TotalPages = (int)Math.Ceiling(TotalOrders / (double)PageSize);
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages == 0 ? 1 : TotalPages));

            RecentOrders = ordersList
                .OrderByDescending(o => o.OrderDate)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            var medicines = await _medicineService.GetAllMedicinesAsync();
            AvailableMedicines = medicines.Count(m => m.IsAvailable);
        }
        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            await _orderService.UpdateOrderStatusAsync(id, "Confirmed");
            TempData["SuccessMessage"] = $"Order #{id} has been accepted successfully!";
            return RedirectToPage();
        }

    }
}