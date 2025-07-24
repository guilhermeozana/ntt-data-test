using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeveloperStore.Users.Domain.Entities;

namespace DeveloperStore.Users.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.Firstname).HasColumnName("Firstname").HasMaxLength(50);
            name.Property(n => n.Lastname).HasColumnName("Lastname").HasMaxLength(50);
        });

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.City).HasColumnName("City").HasMaxLength(50);
            address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(100);
            address.Property(a => a.Number).HasColumnName("Number");
            address.Property(a => a.Zipcode).HasColumnName("Zipcode").HasMaxLength(20);

            address.OwnsOne(a => a.Geolocation, geo =>
            {
                geo.Property(g => g.Lat).HasColumnName("GeoLat").HasMaxLength(30);
                geo.Property(g => g.Long).HasColumnName("GeoLong").HasMaxLength(30);
            });
        });
    }
}