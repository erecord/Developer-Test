using System;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Exceptions;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class QueryProductIdsInBasketCommand : IQueryProductIdsInBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IBasketRepository _basketRepository;

        public QueryProductIdsInBasketCommand(
            IBasketProductRepository basketProductRepository,
            IBasketRepository basketRepository
            )
        {
            _basketProductRepository = basketProductRepository;
            _basketRepository = basketRepository;
        }

        public async Task<IQueryable<int>> Execute(int basketId)
        {
            var basket = await _basketRepository.GetOneAsync(basketId);
            if (basket == null)
                throw new BasketNotFoundException(basketId);

            var basketProducts = await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(basketId));
            var productIdsInBasket = basketProducts.Select(bp => bp.ProductId);
            return productIdsInBasket;
        }
    }
}