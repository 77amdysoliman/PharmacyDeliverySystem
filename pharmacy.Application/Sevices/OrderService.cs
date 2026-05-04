using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Entites;
using pharmacy.domin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.Sevices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // بيجيب كل الطلبات
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync();
            var customers = await _unitOfWork.Users.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();
            var medicines = await _unitOfWork.Medicines.GetAllAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalPrice = o.TotalPrice,
                DeliveryAddress = o.DeliveryAddress,
                Notes = o.Notes,
                UserName = customers.FirstOrDefault(c => c.Id == o.UserId)?.FullName ?? "",
                PharmacyName = pharmacies.FirstOrDefault(p => p.Id == o.PharmacyId)?.Name ?? "",
                Items = orderItems
                    .Where(oi => oi.OrderId == o.Id)
                    .Select(oi => new OrderItemDto
                    {
                        MedicineId = oi.MedicineId,
                        MedicineName = medicines.FirstOrDefault(m => m.Id == oi.MedicineId)?.Name ?? "",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
            });
        }

        // بيجيب طلبات User معين
        public async Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();

            return orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    TotalPrice = o.TotalPrice,
                    DeliveryAddress = o.DeliveryAddress,
                    Notes = o.Notes,
                    UserName = users.FirstOrDefault(c => c.Id == o.UserId)?.FullName ?? "",
                    PharmacyName = pharmacies.FirstOrDefault(p => p.Id == o.PharmacyId)?.Name ?? "",
                    Items = orderItems
                        .Where(oi => oi.OrderId == o.Id)
                        .Select(oi => new OrderItemDto
                        {
                            MedicineId = oi.MedicineId,
                            MedicineName = medicines.FirstOrDefault(m => m.Id == oi.MedicineId)?.Name ?? "",
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice,
                        }).ToList()
                });
        }

        // بيجيب طلبات Pharmacy معينة
        public async Task<IEnumerable<OrderDto>> GetOrdersByPharmacyAsync(int pharmacyId)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var customers = await _unitOfWork.Users.GetAllAsync();

            return orders
                .Where(o => o.PharmacyId == pharmacyId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    TotalPrice = o.TotalPrice,
                    DeliveryAddress = o.DeliveryAddress,
                    Notes = o.Notes,
                    UserName = customers.FirstOrDefault(c => c.Id == o.UserId)?.FullName ?? "",
                    PharmacyName = pharmacies.FirstOrDefault(p => p.Id == o.PharmacyId)?.Name ?? "",
                    Items = orderItems
                        .Where(oi => oi.OrderId == o.Id)
                        .Select(oi => new OrderItemDto
                        {
                            MedicineId = oi.MedicineId,
                            MedicineName = medicines.FirstOrDefault(m => m.Id == oi.MedicineId)?.Name ?? "",
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice,
                        }).ToList()
                });
        }

        // بيجيب طلب بالـ Id
        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var o = await _unitOfWork.Orders.GetByIdAsync(id);
            if (o == null) return null;

            var orderItems = await _unitOfWork.OrderItems.GetAllAsync();
            var medicines = await _unitOfWork.Medicines.GetAllAsync();
            var users = await _unitOfWork.Users.GetAllAsync();
            var pharmacies = await _unitOfWork.Pharmacies.GetAllAsync();

            return new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status.ToString(),
                TotalPrice = o.TotalPrice,
                DeliveryAddress = o.DeliveryAddress,
                Notes = o.Notes,
                UserName = users.FirstOrDefault(c => c.Id == o.UserId)?.FullName ?? "",
                PharmacyName = pharmacies.FirstOrDefault(p => p.Id == o.PharmacyId)?.Name ?? "",
                Items = orderItems
                    .Where(oi => oi.OrderId == o.Id)
                    .Select(oi => new OrderItemDto
                    {
                        MedicineId = oi.MedicineId,
                        MedicineName = medicines.FirstOrDefault(m => m.Id == oi.MedicineId)?.Name ?? "",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
            };
        }

        // بيعمل طلب جديد
        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = orderDto.Items.Sum(i => i.Quantity * i.UnitPrice),
                DeliveryAddress = orderDto.DeliveryAddress,
                Notes = orderDto.Notes,
                UserId = orderDto.UserId,     
                PharmacyId = orderDto.PharmacyId, 
                UserId = orderDto.UserId,
                PharmacyId = orderDto.PharmacyId,
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            foreach (var item in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MedicineId = item.MedicineId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            await _unitOfWork.CompleteAsync();

            orderDto.Id = order.Id;
            orderDto.Status = order.Status.ToString();
            orderDto.TotalPrice = order.TotalPrice;

            return orderDto;
        }

        // بيغير Status الطلب
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return;

            order.Status = Enum.Parse<OrderStatus>(status);
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();
        }
    }
}
