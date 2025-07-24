using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeveloperStore.Sales.Domain.Entities;
namespace DeveloperStore.Sales.Infrastructure.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired();

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.IsCancelled)
            .IsRequired();

        builder.OwnsMany(s => s.Products, item =>
        {
            item.WithOwner().HasForeignKey("SaleId");

            item.Property(i => i.ProductId).IsRequired();
            item.Property(i => i.Quantity).IsRequired();
            item.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            item.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
            item.Property(i => i.Discount).HasColumnType("decimal(18,2)");
            item.Property(i => i.DiscountRate).HasColumnType("decimal(5,2)");

            item.HasKey("SaleId", "ProductId");
        });

        builder.Navigation(s => s.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}