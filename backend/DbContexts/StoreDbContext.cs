using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Models;

namespace StoreBackend.DbContexts
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<BasketProduct>().HasKey(bp => new { bp.BasketId, bp.ProductId });
        }

        public static void RegisterService(IServiceCollection services)
        {

            services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("StoreDbContextConnection")));

        }
    }
}