using StoreBackend.DTOs;
using StoreBackend.Models;

namespace StoreBackend.Extensions
{
    public static class BasketExtensions
    {
        public static BasketDTO ToDTO(this Basket basket) =>
        new BasketDTO
        {
            userId = basket.userId,
            discountId = basket.discountId,
            User = basket.User,
            Discount = basket.Discount,
            Products = basket.Products
        };
    }
}