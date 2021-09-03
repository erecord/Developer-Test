using System.Linq;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IQueryProductIdsInBasketCommand : ICommandWithReturnAndParam<IQueryable<int>, int>
    {
    }
}