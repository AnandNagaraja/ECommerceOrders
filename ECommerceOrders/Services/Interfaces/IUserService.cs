using System.Threading.Tasks;
using ECommerceOrders.Models;

namespace ECommerceOrders.Services
{
    public interface IUserService
    {
        Task<Customer> GetCustomerAsync(string customerEmailId);
    }
}