using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Extensions
{
    public static class BasketProductExtensions
    {

        public static async Task<IQueryable<BasketProduct>> QueryByBasketIdAsync(this IBasketProductRepository repository, int basketId)
            => await repository.WhereAsync(bp => bp.BasketId.Equals(basketId));

        public static async Task<IQueryable<int>> QueryProductIdsInBasketAsync(this IBasketProductRepository repository, int basketId)
            => (await QueryByBasketIdAsync(repository, basketId)).Select(p => p.Id);
    }
}