using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Auth.Models;

namespace StoreBackend.Auth
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }

        public static void RegisterService(IServiceCollection services)
        {

            services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("StoreDbContextConnection")));

        }
    }
}