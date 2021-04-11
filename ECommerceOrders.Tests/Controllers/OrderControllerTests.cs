using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerceOrders.Controllers;
using ECommerceOrders.Exception;
using ECommerceOrders.Models;
using ECommerceOrders.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ECommerceOrders.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {

        private Mock<IOrderService> _orderServiceMock;

        private OrderController _orderController;


        [TestInitialize]
        public void TestInitialize()
        {
            _orderServiceMock = new Mock<IOrderService>();

            _orderController = new OrderController(_orderServiceMock.Object);
        }

        [TestMethod]
        [DataRow("", "tests@test.com")]
        [DataRow("123abc", "")]
        [DataRow("", "")]
        [DataRow("", "    ")]
        [DataRow("    ", "")]
        public async Task GetOderDetailsByCustomerInfo_Should_Return_BadRequest_For_Invalid_CustomerInfo(string customerId, string user)
        {
            var customerInfo = new CustomerInfo() { CustomerId = customerId, User = user };

            var response = await _orderController.GetOderDetailsByCustomerInfo(customerInfo);
            var result = response as ObjectResult;

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            result.Value.Should().Be("Invalid customer information, please correct and try again");

        }

        [TestMethod]
        public async Task GetOderDetailsByCustomerInfo_Should_Return_NotFound_When_OrderService_Throws_ECommerceValidationException_404()
        {
            var customerInfo = new CustomerInfo() { CustomerId = "customerId123", User = "test.test.com" };
            const string expectedErrorMessage = "Customer Not Found";
            const HttpStatusCode statusCode = HttpStatusCode.NotFound;
            _orderServiceMock.Setup(o => o.GetOrderDetailsByCustomerInfoAsync(customerInfo)).Throws(new ECommerceValidationException(expectedErrorMessage, statusCode));


            var response = await _orderController.GetOderDetailsByCustomerInfo(customerInfo);
            var result = response as ObjectResult;

            // Assert
            result.StatusCode.Should().Be((int)statusCode);
            result.Value.Should().Be(expectedErrorMessage);

        }

        [TestMethod]
        public async Task GetOderDetailsByCustomerInfo_Should_Return_BadRequest_When_OrderService_Throws_ECommerceValidationException_400()
        {
            var customerInfo = new CustomerInfo() { CustomerId = "customerId123", User = "test.test.com" };
            const string expectedErrorMessage = "Email Does not match with CustomerId";
            const HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            _orderServiceMock.Setup(o => o.GetOrderDetailsByCustomerInfoAsync(customerInfo)).Throws(new ECommerceValidationException(expectedErrorMessage, statusCode));


            var response = await _orderController.GetOderDetailsByCustomerInfo(customerInfo);
            var result = response as ObjectResult;

            // Assert
            result.StatusCode.Should().Be((int)statusCode);
            result.Value.Should().Be(expectedErrorMessage);

        }



    }
}
