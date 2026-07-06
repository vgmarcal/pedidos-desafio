using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(item => item.Id);

            builder.Property(item => item.ProductId)
                .IsRequired();

            builder.Property(item => item.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(item => item.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(item => item.Quantity)
                .IsRequired();

            // TotalPrice é calculado (UnitPrice * Quantity); não vira coluna.
            builder.Ignore(item => item.TotalPrice);
        }
    }
}
