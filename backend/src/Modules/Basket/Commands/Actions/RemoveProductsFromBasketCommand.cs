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

            if (productsInBasket.Count().Equals(0))
                throw new InvalidOperationException($"The basket with id {parameters.basketId} either does not exist or has no products to remove");

            foreach (var productIdToRemove in parameters.removedProductIds)
            {
                var basketProduct = productsInBasket.FirstOrDefault(bp => bp.ProductId.Equals(productIdToRemove));

                if (basketProduct == null)
                    throw new InvalidOperationException($"The product with id {productIdToRemove} does not exist in the basket with id {parameters.basketId}");

                await _basketProductRepository.DeleteAsync(basketProduct);
            }
        }
    }
}