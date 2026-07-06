using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Services;
using Pedidos.Domain.Entities;
using Xunit;

namespace Pedidos.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _service = new OrderService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrdersMappedToExpectedJson()
        {
            var order = CreateOrder();
            _repositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Order> { order });

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(1);
            var orderResponse = result[0];
            orderResponse.ClientName.Should().Be("Maria Silva");
            orderResponse.ClientEmail.Should().Be("maria@teste.com");
            orderResponse.IsPaid.Should().BeTrue();
            orderResponse.TotalPrice.Should().Be(200m);
            orderResponse.Items.Should().HaveCount(1);
            orderResponse.Items[0].ProductName.Should().Be("Produto A");
        }

        [Fact]
        public async Task GetByIdAsync_WhenOrderExists_ShouldReturnMappedOrder()
        {
            var order = CreateOrder();
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(order);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.ClientEmail.Should().Be("maria@teste.com");
            result.TotalPrice.Should().Be(200m);
        }

        [Fact]
        public async Task GetByIdAsync_WhenOrderDoesNotExist_ShouldReturnNull()
        {
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Order)null);

            var result = await _service.GetByIdAsync(99);

            result.Should().BeNull();
        }

        private static Order CreateOrder()
        {
            var order = new Order("Maria Silva", "maria@teste.com", isPaid: true);
            var product = new Product("Produto A", 100m) { Id = 10 };
            order.AddOrderItem(product, quantity: 2);

            return order;
        }
    }
}
