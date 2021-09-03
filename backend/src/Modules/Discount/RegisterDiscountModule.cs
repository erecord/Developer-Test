using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;

namespace StoreBackend.Modules
{
    public class RegisterDiscountModule
    {
        public RegisterDiscountModule(IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
        }
    }
}