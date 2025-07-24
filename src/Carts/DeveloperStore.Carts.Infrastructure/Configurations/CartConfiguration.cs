using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.Infrastructure.Configurations;

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

        builder.OwnsMany(c => c.Products, item =>
        {
            item.WithOwner().HasForeignKey("CartId");

            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.Quantity).IsRequired();
        });

        builder.Navigation(c => c.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}