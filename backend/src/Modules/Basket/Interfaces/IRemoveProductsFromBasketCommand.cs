using System.Collections.Generic;
using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IRemoveProductsFromBasketCommand : ICommandWithParam<IEnumerable<int>>
    {
    }
}