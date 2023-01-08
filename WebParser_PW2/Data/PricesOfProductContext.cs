using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebParser_PW2.Data
{
    public class PricesOfProductContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        public DbSet<PriceLog> PriceLogs { get; set; }

        public PricesOfProductContext(DbContextOptions<PricesOfProductContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopProduct>().HasKey(u => new { u.ProductId, u.ShopId });
            modelBuilder.Entity<PriceLog>().HasKey(u => new { u.ProductId, u.ShopId, u.Timer });
        }
    }
}
