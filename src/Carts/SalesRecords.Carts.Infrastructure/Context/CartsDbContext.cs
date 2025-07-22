using Microsoft.EntityFrameworkCore;
using SalesRecords.Carts.Domain.Entities;

namespace SalesRecords.Carts.Infrastructure.Context
{
    public class CartsDbContext : DbContext
    {
        public CartsDbContext(DbContextOptions<CartsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts => Set<Cart>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartsDbContext).Assembly);
        }
    }
}
