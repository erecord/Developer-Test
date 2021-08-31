using System;
using System.Linq;
using StoreBackend.Interfaces;

namespace StoreBackend.Commands
{
    public class QueryProductIdsInBasketCommand : IQueryProductIdsInBasketCommand
    {
        private readonly IQueryProductsInBasketCommand _queryProductsInBasketCommand;

        public QueryProductIdsInBasketCommand(IQueryProductsInBasketCommand queryProductsInBasketCommand)
        {
            _queryProductsInBasketCommand = queryProductsInBasketCommand;
        }

        public IQueryable<int> ProductIdsInBasket { get; private set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is int basketId)
            {
                _queryProductsInBasketCommand.Execute(basketId);
                ProductIdsInBasket = _queryProductsInBasketCommand.ProductsInBasket.Select(p => p.Id);
            }
            else
            {
                throw new InvalidCastException("The parameter is not an int");
            }
        }
    }
}