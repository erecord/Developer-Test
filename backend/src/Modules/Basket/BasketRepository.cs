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

        protected override Basket ModifyNewEntityBeforeUpdate(Basket entityFromDb, Basket newEntity)
        {
            newEntity.discountId = entityFromDb.discountId;
            return newEntity;
        }
    }
}