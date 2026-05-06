using System.Collections.Generic;
using System.Threading.Tasks;
using pharmacy.Application.DTOs;

namespace pharmacy.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string userId);
        Task<IEnumerable<OrderDto>> GetOrdersByPharmacyAsync(int pharmacyId);
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task UpdateOrderStatusAsync(int orderId, string status);

        // Added to match usage in Pages/Cart/Index.cshtml.cs
        Task CreateOrderAsync(OrderDto order);
    }
}