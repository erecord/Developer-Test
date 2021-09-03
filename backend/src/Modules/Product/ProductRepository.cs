using StoreBackend.Common;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Repositories
{

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {

        public ProductRepository(StoreDbContext context) : base(context, context.Product)
        {
        }
    }
}