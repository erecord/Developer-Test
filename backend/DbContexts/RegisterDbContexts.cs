using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StoreBackend.DbContexts
{
    public class RegisterDbContexts
    {
        public RegisterDbContexts(IServiceCollection services)
        {
            services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("StoreDbContextConnection")));
        }
    }
}