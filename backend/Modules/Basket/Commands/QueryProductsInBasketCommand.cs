using System;
using System.Linq;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Commands
{
    public class QueryProductsInBasketCommand : IQueryProductsInBasketCommand
    {
        private readonly StoreDbContext _context;

        public QueryProductsInBasketCommand(StoreDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> ProductsInBasket { get; private set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is int basketId)
            {
                var productIdsInBasket = _context.BasketProducts.Where(bp => bp.BasketId == basketId).Select(bp => bp.ProductId);
                var productsInBasket = _context.Product.Where(p => productIdsInBasket.Contains(p.Id));
                ProductsInBasket = productsInBasket;
            }
            else
            {
                throw new InvalidCastException("The parameter is not an int");
            }
        }

    }
}