using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Extensions
{
    public static class BasketRepositoryExtensions
    {
        public async static Task<Basket> ToPopulatedBasket(this IBasketRepository repository, int basketId)
         => await (await repository.IncludeAsync(b => b.Discount)).FirstOrDefaultAsync(p => p.Id.Equals(basketId));
    }
}