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

        public async Task Execute(IEnumerable<int> removedProductIds)
        {
            var removedBasketProducts = await _basketProductRepository.WhereAsync(bp => removedProductIds.Contains(bp.ProductId));
            await _basketProductRepository.DeleteAsync(removedBasketProducts);
        }
    }
}