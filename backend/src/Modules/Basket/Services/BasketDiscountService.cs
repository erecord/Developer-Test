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
                throw new BasketNotFoundException();

            var discount = await _discountRepository.QueryByDiscountCode(discountCode);
            if (discount == null)
                throw new DiscountCodeNotFoundException();

            basket.discountId = discount.Id;
            await _basketRepository.UpdateAsync(basket);
        }

        public async Task<(decimal basketCostBeforeDiscount, decimal basketCostAfterDiscount)> QueryBasketTotalCostWithDiscountAsync(int basketId)
        {
            var basket = await _basketRepository.GetOneAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException();

            if (basket.discountId == null)
                throw new DiscountCodeNotFoundException();

            var totalCostOfBasket = await _queryTotalCostOfBasketCommand.Execute(basketId);

            var basketCostAfterDiscount =
                _calculateDiscountedPriceCommand.Execute((totalCostOfBasket, basket.Discount.Percentage));

            return (totalCostOfBasket, basketCostAfterDiscount);
        }
    }
}