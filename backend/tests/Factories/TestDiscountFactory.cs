using System;
using StoreBackend.Models;

namespace StoreBackend.Tests.Factories
{
    public static class TestDiscountFactory
    {
        public static Discount CreateDiscount(int discountId, string discountCode, decimal discountPercentage) => new Discount
        {
            Id = discountId,
            Code = discountCode,
            Percentage = discountPercentage,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };
    }
}