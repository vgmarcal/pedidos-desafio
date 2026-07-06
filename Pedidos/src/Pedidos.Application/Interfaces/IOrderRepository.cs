using System.Collections.Generic;
using System.Threading.Tasks;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        void Add(Order order);
        void Remove(Order order);
        Task<int> SaveChangesAsync();
    }
}
