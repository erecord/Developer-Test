using System.Collections.Generic;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IAddProductsToBasketCommand : ICommandWithParamAsync<(int basketId, IEnumerable<int> newProductIds)>
    {
    }
}