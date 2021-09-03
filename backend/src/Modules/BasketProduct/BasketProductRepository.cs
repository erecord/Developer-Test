using StoreBackend.Common;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Repositories
{

    public class BasketProductRepository : BaseRepository<BasketProduct>, IBasketProductRepository
    {

        public BasketProductRepository(StoreDbContext context) : base(context, context.BasketProducts)
        {
        }
    }
}