using System;
using System.Collections.Generic;
using System.Text;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrders.DAL.EntityMapping
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("ORDERITEMS");
            builder.Property(oi => oi.OrderItemId).HasColumnName("ORDERITEMID");
            builder.Property(oi => oi.OrderId).HasColumnName("ORDERID");
            builder.Property(oi => oi.ProductId).HasColumnName("PRODUCTID");
            builder.Property(oi => oi.Returnable).HasColumnName("RETURNABLE");
            builder.Property(oi => oi.Quantity).HasColumnName("QUANTITY");
            builder.Property(oi => oi.Price).HasColumnName("PRICE");

            builder.HasOne<Order>(oi => oi.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.HasOne<Product>(oi => oi.Product)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
    }


}
