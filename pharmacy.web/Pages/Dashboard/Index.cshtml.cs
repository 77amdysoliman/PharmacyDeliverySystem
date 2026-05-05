using Microsoft.AspNetCore.Identity;
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

            if (User.IsInRole("PharmacyAdmin") && user?.PharmacyId != null)
            {
                // ✅ بيجيب أوردرات صيدليته بس
                var allOrders = await _orderService.GetOrdersByPharmacyAsync(user.PharmacyId.Value);
                RecentOrders = allOrders.TakeLast(10).Reverse();
                NewOrders = allOrders.Count(o => o.Status == "Pending");
                PendingOrders = allOrders.Count(o => o.Status == "Preparing");
                TodaySales = allOrders
                    .Where(o => o.OrderDate.Date == DateTime.Today)
                    .Sum(o => o.TotalPrice);
            }
            else
            {
                // SuperAdmin بيشوف كل حاجة
                var allOrders = await _orderService.GetAllOrdersAsync();
                RecentOrders = allOrders.TakeLast(10).Reverse();
                NewOrders = allOrders.Count(o => o.Status == "Pending");
                PendingOrders = allOrders.Count(o => o.Status == "Preparing");
                TodaySales = allOrders
                    .Where(o => o.OrderDate.Date == DateTime.Today)
                    .Sum(o => o.TotalPrice);
            }

            var medicines = await _medicineService.GetAllMedicinesAsync();
            AvailableMedicines = medicines.Count(m => m.IsAvailable);
        }

    }
}