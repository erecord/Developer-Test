using System.Linq;
using System.Windows.Input;

namespace StoreBackend.Interfaces
{
    public interface IQueryProductIdsInBasketCommand : ICommand
    {
        IQueryable<int> ProductIdsInBasket { get; }
    }
}