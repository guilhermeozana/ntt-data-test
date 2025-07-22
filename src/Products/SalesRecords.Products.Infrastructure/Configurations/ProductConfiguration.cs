using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesRecords.Products.Domain.Entities;

namespace SalesRecords.Products.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Category)
                .HasMaxLength(50);
            
            builder.OwnsOne(p => p.Rating, rating =>
            {
                rating.Property(r => r.Rate)
                    .HasColumnName("Rating_Rate")
                    .HasColumnType("decimal(3,2)");

                rating.Property(r => r.Count)
                    .HasColumnName("Rating_Count");
            });
        }
    }
}
