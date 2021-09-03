using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class QueryProductIdsInBasketCommand : IQueryProductIdsInBasketCommand
    {
        private readonly IBasketProductRepository _basketProductRepository;

        public QueryProductIdsInBasketCommand(IBasketProductRepository basketProductRepository)
        {
            _basketProductRepository = basketProductRepository;
        }

        public async Task<IQueryable<int>> Execute(int basketId)
        {
            var productIdsInBasket = (await _basketProductRepository.WhereAsync(bp => bp.BasketId == basketId)).Select(bp => bp.ProductId);
            return productIdsInBasket;
        }
    }
}