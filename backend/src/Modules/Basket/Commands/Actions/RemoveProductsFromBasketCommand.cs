using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class RemoveProductsFromBasketCommand : IRemoveProductsFromBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;

        public RemoveProductsFromBasketCommand(IBasketProductRepository basketProductRepository)
        {
            _basketProductRepository = basketProductRepository;
        }

        public async Task Execute((int basketId, IEnumerable<int> removedProductIds) parameters)
        {
            var productsInBasket = await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(parameters.basketId));

            foreach (var productIdToRemove in parameters.removedProductIds)
            {
                var basketProduct = productsInBasket.First(bp => bp.ProductId.Equals(productIdToRemove));

                if (basketProduct.Equals(null))
                    throw new InvalidOperationException($"The product with id {productIdToRemove} does not exist in the basket with id {parameters.basketId}");

                await _basketProductRepository.DeleteAsync(basketProduct);
            }
        }
    }
}