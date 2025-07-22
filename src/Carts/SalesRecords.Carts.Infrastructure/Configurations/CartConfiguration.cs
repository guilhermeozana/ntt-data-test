using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesRecords.Carts.Domain.Entities;

namespace SalesRecords.Carts.Infrastructure.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.Date)
            .IsRequired();

        builder.OwnsMany(c => c.Items, item =>
        {
            item.WithOwner().HasForeignKey("CartId");

            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.Quantity).IsRequired();
        });

        builder.Navigation(c => c.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}