using Microsoft.Extensions.DependencyInjection;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;

namespace StoreBackend.Modules
{
    public class RegisterProductModule
    {
        public RegisterProductModule(IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
        }
    }
}