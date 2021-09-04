using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class AddProductsToBasketCommand : IAddProductsToBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBasketProductFactory _basketProductFactory;

        public AddProductsToBasketCommand(
            IBasketProductRepository basketProductRepository,
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            IBasketProductFactory basketProductFactory)
        {
            _basketProductRepository = basketProductRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _basketProductFactory = basketProductFactory;
        }

        public async Task Execute((int basketId, IEnumerable<int> newProductIds) parameters)
        {
            await Task.Run(async () =>
            {
                var basket = await _basketRepository.GetOneAsync(parameters.basketId);
                if (basket == null)
                    throw new InvalidOperationException($"The basket with id '{parameters.basketId}' does not exist.");

                foreach (var productId in parameters.newProductIds.ToList())
                {
                    var product = await _productRepository.GetOneAsync(productId);
                    if (product == null)
                        throw new InvalidOperationException($"The product with id '{productId}' does not exist.");

                    await _basketProductRepository.CreateAsync(_basketProductFactory.CreateBasketProduct(parameters.basketId, productId));
                }
            });
        }
    }
}