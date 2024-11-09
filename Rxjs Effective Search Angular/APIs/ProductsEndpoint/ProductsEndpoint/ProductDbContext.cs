using Microsoft.EntityFrameworkCore;

namespace ProductsEndpoint
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().ToTable("Product", "Production");
        }

        public DbSet<Product> Products { get; set; }
    }
}
