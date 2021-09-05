using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Commands;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;

namespace StoreBackend.Modules
{
    public class RegisterDiscountModule
    {
        public RegisterDiscountModule(IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddTransient<ICalculateDiscountedPriceCommand, CalculateDiscountedPriceCommand>();
        }
    }
}