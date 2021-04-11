using System;
using System.Linq;
using System.Reflection;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace ECommerceOrders.DAL
{
    public class ReadOnlyECommerceDbContext : BaseContext<ReadOnlyECommerceDbContext>, IReadOnlyECommerceDbContext
    {

        public ReadOnlyECommerceDbContext(DbContextOptions<ReadOnlyECommerceDbContext> options)
            : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyEntityConfigurationFromCurrentAssembly(modelBuilder);
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }


        private void ApplyEntityConfigurationFromCurrentAssembly(ModelBuilder modelBuilder)
        {
            var mappingTypes = GetType().Assembly.GetTypes().Where(t => t.GetInterfaces()
                .Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

            foreach (var type in mappingTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

        }

    }
}