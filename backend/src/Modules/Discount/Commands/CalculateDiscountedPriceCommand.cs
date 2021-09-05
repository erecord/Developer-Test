using System;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class CalculateDiscountedPriceCommand : ICalculateDiscountedPriceCommand
    {
        public decimal Execute((decimal originalPrice, decimal discountPercentage) parameters)
        {
            var discountPercentageOutOfBounds = parameters.discountPercentage > 100 || parameters.discountPercentage < 0;
            if (discountPercentageOutOfBounds)
                throw new InvalidOperationException();

            var scaledDiscountPercentage = parameters.discountPercentage / 100;
            var discountSavings = parameters.originalPrice * scaledDiscountPercentage;

            var totalCostOfBasketWithDiscount = parameters.originalPrice - discountSavings;
            return totalCostOfBasketWithDiscount;
        }
    }
}