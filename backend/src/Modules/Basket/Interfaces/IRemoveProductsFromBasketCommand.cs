using System.Collections.Generic;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IRemoveProductsFromBasketCommand : ICommandWithParamAsync<(int basketId, IEnumerable<int> removedProductIds)>
    {
    }
}