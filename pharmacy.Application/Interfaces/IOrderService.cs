using pharmacy.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

        
        Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(int userId);

        Task<IEnumerable<OrderDto>> GetOrdersByPharmacyAsync(int pharmacyId);

        Task<OrderDto?> GetOrderByIdAsync(int id);

        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);

        Task UpdateOrderStatusAsync(int orderId, string status);
    }
}
