using System;
using System.Collections.Generic;
using System.Text;
using ECommerceOrders.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrders.DAL.EntityMapping
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("PRODUCTS");

            builder.HasKey(p => p.ProductId);

            builder.Property(oi => oi.ProductId).HasColumnName("PRODUCTID");
            builder.Property(oi => oi.ProductName).HasColumnName("PRODUCTNAME");
            builder.Property(oi => oi.Colour).HasColumnName("COLOUR");
            builder.Property(oi => oi.PackHeight).HasColumnName("PACKHEIGHT");
            builder.Property(oi => oi.PackWeight).HasColumnName("PACKWEIGHT");
            builder.Property(oi => oi.PackWidth).HasColumnName("PACKWIDTH");
            builder.Property(oi => oi.Size).HasColumnName("SIZE");

        }
    }
}
