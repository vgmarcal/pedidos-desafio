using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pedidos.Application.DTOs.Orders;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Mappings;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();

            return orders.Select(order => OrderMapper.ToResponse(order)).ToList();
        }

        public async Task<OrderResponse> GetByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                return null;

            return OrderMapper.ToResponse(order);
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var order = new Order(request.ClientName, request.ClientEmail, request.IsPaid);

            foreach (var itemRequest in request.Items)
            {
                order.AddOrderItem(OrderMapper.ToProduct(itemRequest), itemRequest.Quantity);
            }

            _repository.Add(order);
            await _repository.SaveChangesAsync();

            return OrderMapper.ToResponse(order);
        }

        public async Task<OrderResponse> UpdateAsync(int id, UpdateOrderRequest request)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                return null;

            order.UpdateClient(request.ClientName, request.ClientEmail);
            order.SetPaid(request.IsPaid);

            // O PUT substitui a lista de itens inteira (remove os antigos e grava os novos).
            order.ClearOrderItems();
            foreach (var itemRequest in request.Items)
            {
                order.AddOrderItem(OrderMapper.ToProduct(itemRequest), itemRequest.Quantity);
            }

            await _repository.SaveChangesAsync();

            return OrderMapper.ToResponse(order);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null)
                return false;

            _repository.Remove(order);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
