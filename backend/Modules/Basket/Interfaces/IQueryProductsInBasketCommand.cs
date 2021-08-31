using System.Linq;
using System.Windows.Input;
using StoreBackend.Models;

namespace StoreBackend.Interfaces
{
    public interface IQueryProductsInBasketCommand : ICommand
    {
        IQueryable<Product> ProductsInBasket { get; }
    }
}