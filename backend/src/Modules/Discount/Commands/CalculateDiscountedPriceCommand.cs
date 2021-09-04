using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class CalculateDiscountedPriceCommand : ICalculateDiscountedPriceCommand
    {
        public decimal Execute((decimal originalPrice, decimal discountPercentage) parameters)
        {
            var scaledDiscountPercentage = parameters.discountPercentage / 100;
            var discountSavings = parameters.originalPrice * scaledDiscountPercentage;

            var totalCostOfBasketWithDiscount = parameters.originalPrice - discountSavings;
            return totalCostOfBasketWithDiscount;
        }
    }
}