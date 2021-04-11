using System.Threading.Tasks;
using ECommerceOrders.DAL.Entities;
using ECommerceOrders.Models;
using ECommerceOrders.Repositories;
using ECommerceOrders.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FizzWare.NBuilder;

namespace ECommerceOrders.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IUserService> _userServiceMock;

        private OrderService _orderService;

        [TestInitialize]
        public void TestInitialize()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _userServiceMock = new Mock<IUserService>();

            _orderService = new OrderService(_orderRepositoryMock.Object, _userServiceMock.Object);
        }

        [TestMethod]
        public async Task GetOrderDetailsByCustomerInfoAsync_ShouldThrow_ECommerceValidationException_404_When_UserService_ReturnsNull()
        {
            var customerInfo = new CustomerInfo() { CustomerId = "Customer123", User = "Test@Test.com" };
            _userServiceMock.Setup(u => u.GetCustomerAsync(customerInfo.User))
                .Returns(Task.FromResult<Customer>(null));

            _orderRepositoryMock.Setup(o => o.GetOrderByCustomerIdAsync(customerInfo.CustomerId)).Returns(Task.FromResult(new Order()));

            try
            {
                var response = _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo).Result;
                Assert.Fail("Test Should Fail");
            }
            catch (System.Exception e)
            {
                e.InnerException.Message.Should().Be("Customer does not exists " + customerInfo.User);
            }
        }

        [TestMethod]
        public async Task GetOrderDetailsByCustomerInfoAsync_ShouldThrow_ECommerceValidationException_400_When_UserService_CustomerIDNotMatch()
        {
            var customerInfo = new CustomerInfo() { CustomerId = "Customer123", User = "Test@Test.com" };
            _userServiceMock.Setup(u => u.GetCustomerAsync(customerInfo.User))
                .Returns(Task.FromResult(new Customer() { CustomerId = "NotMatch" }));

            _orderRepositoryMock.Setup(o => o.GetOrderByCustomerIdAsync(customerInfo.CustomerId)).Returns(Task.FromResult(new Order()));

            try
            {
                var response = _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo).Result;
                Assert.Fail("Test Should Fail");
            }
            catch (System.Exception e)
            {
                e.InnerException.Message.Should().Be("Customer Id does not match with Email ID");
            }
        }

        [TestMethod]
        public async Task GetOrderDetailsByCustomerInfoAsync_WithValid_Input_Should_Return_OrderDetails()
        {

            var customerInfo = new CustomerInfo() { CustomerId = "Customer123", User = "Test@Test.com" };
            var product = Builder<Product>.CreateNew().Build();
            var orderItems = Builder<OrderItem>.CreateListOfSize(2).All().With(p => p.Product = product).Build();
            var order = Builder<Order>.CreateNew().With(o => o.OrderItems = orderItems).Build();
            var customer = Builder<Customer>.CreateNew().Build();
            customer.CustomerId = customerInfo.CustomerId;

            _userServiceMock.Setup(u => u.GetCustomerAsync(customerInfo.User))
                .Returns(Task.FromResult(customer));

            _orderRepositoryMock.Setup(o => o.GetOrderByCustomerIdAsync(customerInfo.CustomerId)).Returns(Task.FromResult(order));

            var response = _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo).Result;

            response.Should().NotBeNull();
            response.Order.Should().NotBeNull();
            response.Customer.Should().NotBeNull();
            response.Order.OrderItems.Should().NotBeNull();

        }

        [TestMethod]
        public async Task GetOrderDetailsByCustomerInfoAsync_With_Null_Order_Should_Return_OnlyCustomerInfo()
        {

            var customerInfo = new CustomerInfo() { CustomerId = "Customer123", User = "Test@Test.com" };
            var customer = Builder<Customer>.CreateNew().Build();
            customer.CustomerId = customerInfo.CustomerId;

            _userServiceMock.Setup(u => u.GetCustomerAsync(customerInfo.User))
                .Returns(Task.FromResult(customer));

            _orderRepositoryMock.Setup(o => o.GetOrderByCustomerIdAsync(customerInfo.CustomerId)).Returns(Task.FromResult<Order>(null));

            var response = _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo).Result;

            response.Should().NotBeNull();
            response.Customer.Should().NotBeNull();
            response.Order.Should().BeNull();

        }

        [TestMethod]
        public async Task GetOrderDetailsByCustomerInfoAsync_With_Null_OrderItem_Should_Return_OrderDetails_without_OrderItem()
        {

            var customerInfo = new CustomerInfo() { CustomerId = "Customer123", User = "Test@Test.com" };

            var order = Builder<Order>.CreateNew().Build();
            var customer = Builder<Customer>.CreateNew().Build();
            customer.CustomerId = customerInfo.CustomerId;

            _userServiceMock.Setup(u => u.GetCustomerAsync(customerInfo.User))
                .Returns(Task.FromResult(customer));

            _orderRepositoryMock.Setup(o => o.GetOrderByCustomerIdAsync(customerInfo.CustomerId)).Returns(Task.FromResult(order));

            var response = _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo).Result;

            response.Should().NotBeNull();
            response.Order.Should().NotBeNull();
            response.Customer.Should().NotBeNull();
            response.Order.OrderItems.Should().BeNull();

        }


    }
}
