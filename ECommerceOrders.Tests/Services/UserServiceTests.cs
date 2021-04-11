using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ECommerceOrders.Configuration;
using ECommerceOrders.Models;
using ECommerceOrders.Services;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ECommerceOrders.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IConfigurationRepository> _configurationRepositoryMock;
        private const string BaseUrl = "http://test.com";
        private const string ApiKey = "VeryStrongAPIKey";

        private IUserService _userService;
        [TestInitialize]
        public void TestInitialize()
        {
            _configurationRepositoryMock = new Mock<IConfigurationRepository>();
            _configurationRepositoryMock.Setup(c => c.GetBaseUrl()).Returns(BaseUrl);
            _configurationRepositoryMock.Setup(c => c.GetApiKey()).Returns(ApiKey);
        }

        [TestMethod]
        public async Task GetCustomerAsync_Should_Return_Valid_Customer()
        {

            var returnCustomerObject = Builder<Customer>.CreateNew().Build();
            var returnCustomerJson = JsonConvert.SerializeObject(returnCustomerObject);
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(returnCustomerJson),
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl),
            };
            _userService = new UserService(httpClient, _configurationRepositoryMock.Object);

            var response = _userService.GetCustomerAsync(returnCustomerObject.Email).Result;

            response.Email.Should().Be(returnCustomerObject.Email);
            response.FirstName.Should().Be(returnCustomerObject.FirstName);
            response.LastName.Should().Be(returnCustomerObject.LastName);
            response.LastLoggedIn.Should().Be(returnCustomerObject.LastLoggedIn);
            response.PreferredLanguage.Should().Be(returnCustomerObject.PreferredLanguage);

        }


        [TestMethod]
        public async Task GetCustomerAsync_Should_Throw_EcommerceValidationException_When_ClientAPI_Response_Is_BadRequest()
        {

            var returnCustomerObject = Builder<Customer>.CreateNew().Build();
            var returnCustomerJson = JsonConvert.SerializeObject(returnCustomerObject);
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(returnCustomerJson),
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl),
            };
            _userService = new UserService(httpClient, _configurationRepositoryMock.Object);

            try
            {

                var response = _userService.GetCustomerAsync(returnCustomerObject.Email).Result;
                Assert.Fail("Test Should Fail");
            }
            catch (System.Exception e)
            {
                e.InnerException.Message.Should().Be("Customer Email does not exists");
            }


        }

    }
}
