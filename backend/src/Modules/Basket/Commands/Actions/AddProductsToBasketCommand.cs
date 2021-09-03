using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class AddProductsToBasketCommand : IAddProductsToBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IBasketProductFactory _basketProductFactory;

        public AddProductsToBasketCommand(IBasketProductRepository basketProductRepository, IBasketProductFactory basketProductFactory)
        {
            _basketProductRepository = basketProductRepository;
            _basketProductFactory = basketProductFactory;
        }

        public async Task Execute((int basketId, IEnumerable<int> newProductIds) parameters)
        {
            await Task.Run(() =>
                parameters.newProductIds.ToList().ForEach(async productId =>
                {
                    await _basketProductRepository.CreateAsync(_basketProductFactory.CreateBasketProduct(parameters.basketId, productId));
                })
            );
        }
    }
}