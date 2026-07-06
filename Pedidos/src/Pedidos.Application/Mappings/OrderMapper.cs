using System.Linq;
using Pedidos.Application.DTOs.Orders;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Mappings
{
    /// <summary>
    /// Conversão manual entre entidades de domínio e DTOs (request/response).
    /// </summary>
    public static class OrderMapper
    {
        public static OrderResponse ToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                ClientName = order.ClientName,
                ClientEmail = order.ClientEmail,
                IsPaid = order.IsPaid,
                TotalPrice = order.TotalPrice,
                Items = order.OrderItems.Select(item => ToResponse(item)).ToList()
            };
        }

        public static OrderItemResponse ToResponse(OrderItem item)
        {
            return new OrderItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            };
        }

        public static Product ToProduct(OrderItemRequest request)
        {
            return new Product(request.ProductName, request.UnitPrice) { Id = request.ProductId };
        }
    }
}
