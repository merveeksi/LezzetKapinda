using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class OrderItemConfiguration<TId> : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        
        builder.Property(oi => oi.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(oi => oi.Quantity)
            .IsRequired();
        
        builder.Property(oi => oi.Price)
            .IsRequired();

        builder.Property(oi => oi.Total)
            .IsRequired();

        builder.HasOne(oi => oi.Product)
            .WithMany(f => f.OrderItems)
            .HasForeignKey(oi => oi.ProductId);
        
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);
        
        builder.ToTable("order_items");
    }
} 