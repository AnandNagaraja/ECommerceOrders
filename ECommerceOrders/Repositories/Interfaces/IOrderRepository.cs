using System.Threading.Tasks;
using ECommerceOrders.DAL.Entities;

namespace ECommerceOrders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByCustomerIdAsync(string customerId);
    }
}