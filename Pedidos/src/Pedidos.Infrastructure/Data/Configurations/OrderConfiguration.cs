using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(order => order.Id);

            builder.Property(order => order.ClientName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(order => order.ClientEmail)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(order => order.IsPaid)
                .IsRequired();

            builder.Property(order => order.OrderDate)
                .IsRequired();

            // TotalPrice é calculado em memória a partir dos itens; não vira coluna.
            builder.Ignore(order => order.TotalPrice);

            builder.HasMany(order => order.OrderItems)
                .WithOne()
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
