using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.Interfaces;
using pharmacy.Application.DTOs;

namespace Pharmacy.web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMedicineService _medicineService;

        public int NewOrders { get; set; }
        public int PendingOrders { get; set; }
        public int AvailableMedicines { get; set; }
        public decimal TodaySales { get; set; }
        public IEnumerable<OrderDto> RecentOrders { get; set; } = new List<OrderDto>();

        public IndexModel(IOrderService orderService, IMedicineService medicineService)
        {
            _orderService = orderService;
            _medicineService = medicineService;
        }

        public async Task OnGetAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var medicines = await _medicineService.GetAllMedicinesAsync();

            NewOrders = orders.Count(o => o.Status == "Pending");
            PendingOrders = orders.Count(o => o.Status == "Preparing");
            AvailableMedicines = medicines.Count(m => m.IsAvailable);
            TodaySales = orders
                .Where(o => o.OrderDate.Date == DateTime.Today)
                .Sum(o => o.TotalPrice);
            RecentOrders = orders.TakeLast(10).Reverse();
        }

    }
}