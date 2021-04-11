using System.Threading.Tasks;
using ECommerceOrders.Models;
using ECommerceOrders.Models.OrderDetails;

namespace ECommerceOrders.Services
{
    public interface IOrderService
    {
        Task<OrderDetails> GetOrderDetailsByCustomerInfoAsync(CustomerInfo customerInfo);
    }
}