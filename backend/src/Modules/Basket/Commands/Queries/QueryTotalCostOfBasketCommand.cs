using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class QueryTotalCostOfBasketCommand : IQueryTotalCostOfBasketCommand
    {
        private readonly IQueryProductsInBasketCommand _queryProductsInBasketCommand;

        public QueryTotalCostOfBasketCommand(
            IQueryProductsInBasketCommand queryProductsInBasketCommand
            )
        {
            _queryProductsInBasketCommand = queryProductsInBasketCommand;
        }
        public async Task<decimal> Execute(int basketId)
        {
            decimal totalCostOfBasket = 0;
            var productIdsInBasket = (await _queryProductsInBasketCommand.Execute(basketId)).ToList();

            productIdsInBasket.ForEach(p => totalCostOfBasket += p.Price);

            return totalCostOfBasket;
        }
    }
}