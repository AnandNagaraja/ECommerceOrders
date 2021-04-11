using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrders.DAL
{
    public interface IReadOnlyECommerceDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Product> Products { get; set; }

    }
}