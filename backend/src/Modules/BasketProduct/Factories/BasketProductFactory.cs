using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Factories
{
    public class BasketProductFactory : IBasketProductFactory
    {
        public BasketProduct CreateBasketProduct(int basketId, int productId)
            => new BasketProduct { BasketId = basketId, ProductId = productId };
    }
}