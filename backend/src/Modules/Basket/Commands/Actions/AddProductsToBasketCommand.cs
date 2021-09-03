using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Commands
{
    public class AddProductsToBasketCommand : IAddProductsToBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;

        public AddProductsToBasketCommand(IBasketProductRepository basketProductRepository)
        {
            _basketProductRepository = basketProductRepository;
        }

        public async Task Execute((int basketId, IEnumerable<int> newProductIds) parameters)
        {
            await Task.Run(() =>
                parameters.newProductIds.ToList().ForEach(async productId =>
                {
                    await _basketProductRepository.CreateAsync(new BasketProduct { BasketId = parameters.basketId, ProductId = productId });
                })
            );
        }
    }
}