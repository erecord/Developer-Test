using System.Linq;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IQueryProductIdsInBasketCommand : ICommandWithReturnAndParamAsync<IQueryable<int>, int>
    {
    }
}