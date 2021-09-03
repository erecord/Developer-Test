using StoreBackend.Common;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Repositories
{
    public class BasketRepository : BaseRepository<Basket>, IBasketRepository
    {

        public BasketRepository(StoreDbContext context) : base(context, context.Basket)
        {
        }
    }
}