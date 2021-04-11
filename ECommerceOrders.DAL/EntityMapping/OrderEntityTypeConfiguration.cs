using System;
using System.Collections.Generic;
using System.Text;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrders.DAL.EntityMapping
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("ORDERS");
            builder.HasKey(o => o.OrderId);
            builder.Property(o => o.ContainsGift).HasColumnName("CONTAINSGIFT");
            builder.Property(o => o.CustomerId).HasColumnName("CUSTOMERID");
            builder.Property(o => o.DeliveryExpected).HasColumnName("DELIVERYEXPECTED");
            builder.Property(o => o.OrderDate).HasColumnName("ORDERDATE");
            builder.Property(o => o.OrderSource).HasColumnName("ORDERSOURCE");
            builder.Property(o => o.ShippingMode).HasColumnName("SHIPPINGMODE");


            builder.HasMany<OrderItem>(o => o.OrderItems);
        }
    }
}
