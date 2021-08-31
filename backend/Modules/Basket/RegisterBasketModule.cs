using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Commands;
using StoreBackend.Facade;
using StoreBackend.Interfaces;

namespace StoreBackend.Modules
{
    public class RegisterBasketModule
    {
        public RegisterBasketModule(IServiceCollection services)
        {
            services.AddTransient<IQueryProductsInBasketCommand, QueryProductsInBasketCommand>();
            services.AddTransient<IQueryProductIdsInBasketCommand, QueryProductIdsInBasketCommand>();
            services.AddTransient<BasketControllerFacade>();
        }
    }
}