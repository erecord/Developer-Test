using StoreBackend.DTOs;
using StoreBackend.Models;

namespace StoreBackend.Extensions
{
    public static class BasketExtensions
    {
        public static BasketDTO ToDTO(this Basket basket) =>
            new BasketDTO
            {
                Id = basket.Id,
                userId = basket.userId,
                discountId = basket.discountId,
                User = basket.User,
                Discount = basket.Discount,
                Products = basket.Products
            };

        public static BasketProduct ToBasketProductDTO(this Basket basket, int productId) =>
            new BasketProduct
            {
                BasketId = basket.Id,
                ProductId = productId
            };
    }
}