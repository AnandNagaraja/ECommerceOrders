using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ECommerceOrders.DAL.Entities;
using ECommerceOrders.Exception;
using ECommerceOrders.Models;
using ECommerceOrders.Models.OrderDetails;
using ECommerceOrders.Repositories;

namespace ECommerceOrders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;

        public OrderService(IOrderRepository orderRepository, IUserService userService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
        }

        public async Task<OrderDetails> GetOrderDetailsByCustomerInfoAsync(CustomerInfo customerInfo)
        {

            var customer = _userService.GetCustomerAsync(customerInfo.User);
            var order = _orderRepository.GetOrderByCustomerIdAsync(customerInfo.CustomerId);

            await Task.WhenAll(customer, order);

            if (customer.Result == null)
            {

                throw new ECommerceValidationException($"Customer does not exists {customerInfo.User}",
                    HttpStatusCode.NotFound);
            }

            //Throw an exception if the customer id does not match with the Email id of the customer Info parameter
            if (customer.Result.CustomerId != customerInfo.CustomerId)
            {
                throw new ECommerceValidationException("Customer Id does not match with Email ID", HttpStatusCode.BadRequest);
            }

            return PopulateAndGetOrderDetails(order.Result, customer.Result);
        }

        private static OrderDetails PopulateAndGetOrderDetails(Order order, Customer customer)
        {
            var result = new OrderDetails
            {
                Customer = new CustomerDetails() { FirstName = customer.FirstName, LastName = customer.LastName },
                Order = new CustomerOrder()
            };

            if (order != null)
            {
                result.Order.DeliveryAddress = $"{customer.HouseNumber} {customer.Street}, {customer.Town}, {customer.Postcode}";
                result.Order.OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("dd-MMM-yyyy") : string.Empty;
                result.Order.OrderNumber = order.OrderId;
                result.Order.DeliveryExpected = order.DeliveryExpected.HasValue ? order.DeliveryExpected.Value.ToString("dd-MMM-yyyy") : string.Empty;
                if (order.OrderItems != null)
                {
                    var isGift = order.ContainsGift.GetValueOrDefault(false);
                    result.Order.OrderItems = new List<CustomerOrderItem>();
                    foreach (var orderItem in order.OrderItems)
                    {
                        result.Order.OrderItems.Add(new CustomerOrderItem()
                        {
                            PriceEach = orderItem.Price,
                            Product = isGift ? "Gift" : orderItem.Product.ProductName,
                            Quantity = orderItem.Quantity
                        });
                    }
                }
            }
            return result;

        }
    }
}
