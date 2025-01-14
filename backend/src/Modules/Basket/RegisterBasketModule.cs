using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Commands;
using StoreBackend.Facade;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;
using StoreBackend.Services;

namespace StoreBackend.Modules
{
    public class RegisterBasketModule
    {
        public RegisterBasketModule(IServiceCollection services)
        {
            services.AddTransient<IQueryProductsInBasketCommand, QueryProductsInBasketCommand>();
            services.AddTransient<IQueryProductIdsInBasketCommand, QueryProductIdsInBasketCommand>();
            services.AddTransient<IQueryTotalCostOfBasketCommand, QueryTotalCostOfBasketCommand>();
            services.AddTransient<IRemoveProductsFromBasketCommand, RemoveProductsFromBasketCommand>();
            services.AddTransient<IAddProductsToBasketCommand, AddProductsToBasketCommand>();

            services.AddTransient<IBasketControllerFacade, BasketControllerFacade>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketDiscountService, BasketDiscountService>();
        }
    }
}