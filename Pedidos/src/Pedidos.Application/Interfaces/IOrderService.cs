using System.Collections.Generic;
using System.Threading.Tasks;
using Pedidos.Application.DTOs.Orders;

namespace Pedidos.Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetAllAsync();
        Task<OrderResponse> GetByIdAsync(int id);
        Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse> UpdateAsync(int id, UpdateOrderRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
