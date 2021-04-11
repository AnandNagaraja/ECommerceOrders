using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceOrders.DAL;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrders.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ReadOnlyECommerceDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderByCustomerIdAsync(string customerId)
        {
            return await Context.Set<Order>()
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync(o => o.CustomerId == customerId);
        }

    }
}
