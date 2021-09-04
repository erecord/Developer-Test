using StoreBackend.Models;

namespace StoreBackend.Interfaces
{
    public interface IBasketProductFactory
    {
        BasketProduct CreateBasketProduct(int basketId, int productId);
    }
}