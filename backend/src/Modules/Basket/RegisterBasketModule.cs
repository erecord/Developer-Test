using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Commands;
using StoreBackend.Facade;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;

namespace StoreBackend.Modules
{
    public class RegisterBasketModule
    {
        public RegisterBasketModule(IServiceCollection services)
        {
            services.AddTransient<IQueryProductsInBasketCommand, QueryProductsInBasketCommand>();
            services.AddTransient<IQueryProductIdsInBasketCommand, QueryProductIdsInBasketCommand>();
            services.AddTransient<IRemoveProductsFromBasketCommand, RemoveProductsFromBasketCommand>();
            services.AddTransient<IAddProductsToBasketCommand, AddProductsToBasketCommand>();


            services.AddTransient<BasketControllerFacade>();
            services.AddScoped<IBasketRepository, BasketRepository>();
        }
    }
}