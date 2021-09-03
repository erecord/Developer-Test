using System.Collections.Generic;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IAddProductsToBasketCommand : ICommandWithParam<(int basketId, IEnumerable<int> newProductIds)>
    {
    }
}