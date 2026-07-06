using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pedidos.Api.Controllers;
using Pedidos.Application.DTOs.Orders;
using Pedidos.Application.Interfaces;
using Xunit;

namespace Pedidos.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _serviceMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _serviceMock = new Mock<IOrderService>();
            _controller = new OrderController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithOrderList()
        {
            var orders = new List<OrderResponse>
            {
                new OrderResponse
                {
                    Id = 1,
                    ClientName = "Maria Silva",
                    ClientEmail = "maria@teste.com",
                    IsPaid = true,
                    TotalPrice = 200m,
                    Items = new List<OrderItemResponse>
                    {
                        new OrderItemResponse { Id = 1, ProductId = 10, ProductName = "Produto A", UnitPrice = 100m, Quantity = 2 }
                    }
                }
            };
            _serviceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(orders);

            var result = await _controller.GetAll();

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public async Task GetById_WhenOrderExists_ShouldReturnOkWithOrder()
        {
            var order = new OrderResponse
            {
                Id = 1,
                ClientName = "Maria Silva",
                ClientEmail = "maria@teste.com",
                IsPaid = true,
                TotalPrice = 200m
            };
            _serviceMock
                .Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync(order);

            var result = await _controller.GetById(1);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(order);
        }

        [Fact]
        public async Task GetById_WhenOrderDoesNotExist_ShouldReturnNotFound()
        {
            _serviceMock
                .Setup(service => service.GetByIdAsync(99))
                .ReturnsAsync((OrderResponse)null);

            var result = await _controller.GetById(99);

            result.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
