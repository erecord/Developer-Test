using StoreBackend.Common;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Repositories
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(StoreDbContext context) : base(context, context.Discount)
        {
        }
    }
}