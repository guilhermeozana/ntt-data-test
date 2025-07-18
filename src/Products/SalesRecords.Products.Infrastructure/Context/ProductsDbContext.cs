using Microsoft.EntityFrameworkCore;
using SalesRecords.Products.Domain.Entities;

namespace SalesRecords.Products.Infrastructure.Context
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
        }
    }
}
