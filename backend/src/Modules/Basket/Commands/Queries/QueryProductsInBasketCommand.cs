using System;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Commands
{
    public class QueryProductsInBasketCommand : IQueryProductsInBasketCommand
    {
        private readonly IProductRepository _productRepository;
        private readonly IQueryProductIdsInBasketCommand _queryProductIdsInBasketCommand;

        public QueryProductsInBasketCommand(IProductRepository productRepository, IQueryProductIdsInBasketCommand queryProductIdsInBasketCommand)
        {
            _productRepository = productRepository;
            _queryProductIdsInBasketCommand = queryProductIdsInBasketCommand;
        }

        public async Task<IQueryable<Product>> Execute(int basketId)
        {

            var productIdsInBasket = await _queryProductIdsInBasketCommand.Execute(basketId);
            var productsInBasket = await _productRepository.WhereAsync(p => productIdsInBasket.Contains(p.Id));
            return productsInBasket;
        }

    }
}