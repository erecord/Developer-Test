using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;

namespace StoreBackend.Modules
{
    public class RegisterBasketProductModule
    {
        public RegisterBasketProductModule(IServiceCollection services)
        {
            services.AddScoped<IBasketProductRepository, BasketProductRepository>();
        }
    }
}