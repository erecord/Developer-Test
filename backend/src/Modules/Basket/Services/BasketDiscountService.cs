using System;
using System.Threading.Tasks;
using StoreBackend.Exceptions;
using StoreBackend.Extensions;
using StoreBackend.Interfaces;

namespace StoreBackend.Services
{
    public class BasketDiscountService : IBasketDiscountService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IQueryTotalCostOfBasketCommand _queryTotalCostOfBasketCommand;
        private readonly ICalculateDiscountedPriceCommand _calculateDiscountedPriceCommand;

        public BasketDiscountService(
            IBasketRepository basketRepository,
            IDiscountRepository discountRepository,
            IQueryTotalCostOfBasketCommand queryTotalCostOfBasketCommand,
            ICalculateDiscountedPriceCommand calculateDiscountedPriceCommand
            )
        {
            _basketRepository = basketRepository;
            _discountRepository = discountRepository;
            _queryTotalCostOfBasketCommand = queryTotalCostOfBasketCommand;
            _calculateDiscountedPriceCommand = calculateDiscountedPriceCommand;
        }

        public async Task SetDiscountOnBasketAsync(int basketId, string discountCode)
        {
            var basket = await _basketRepository.GetOneAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException(basketId);

            var discount = await _discountRepository.QueryByDiscountCode(discountCode);
            if (discount == null)
                throw new DiscountCodeNotFoundException(discountCode);

            basket.discountId = discount.Id;
            await _basketRepository.UpdateAsync(basket);
        }

        public async Task<decimal> QueryBasketTotalCostAfterDiscountAsync(int basketId)
        {
            var basket = await _basketRepository.ToPopulatedBasketAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException(basketId);

            var discountId = basket.discountId;
            if (discountId == null)
                throw new InvalidOperationException("The basket does not have a discount code");

            var totalCostOfBasket = await _queryTotalCostOfBasketCommand.Execute(basketId);

            var basketTotalCostAfterDiscount =
                _calculateDiscountedPriceCommand.Execute((totalCostOfBasket, basket.Discount.Percentage));

            return basketTotalCostAfterDiscount;
        }
    }
}