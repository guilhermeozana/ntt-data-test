using Microsoft.EntityFrameworkCore;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.Infrastructure.Context
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
