using System.Collections.Generic;
using StoreBackend.Extensions;
using StoreBackend.Models;

namespace StoreBackend.Tests.Factories
{
    public static class TestBasketFactory
    {
        public static Basket CreateEmptyBasket(int basketId, int userId) => new Basket
        {
            Id = basketId,
            userId = userId
        };

        public static (Basket basket, List<BasketProduct> basketProducts) CreateBasketWithExistingProducts(
            int basketId,
            int userId,
            IEnumerable<Product> products
        )
        {
            var basket = CreateEmptyBasket(basketId, userId);
            var basketProducts = new List<BasketProduct>();

            foreach (var product in products)
            {
                var basketProduct = basket.ToBasketProductDTO(product.Id);
                basketProducts.Add(basketProduct);
            }

            return (basket, basketProducts);
        }
    }
}