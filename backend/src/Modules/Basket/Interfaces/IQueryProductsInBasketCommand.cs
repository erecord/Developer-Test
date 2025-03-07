using System.Linq;
using StoreBackend.Common.Interfaces;
using StoreBackend.Models;

namespace StoreBackend.Interfaces
{
    public interface IQueryProductsInBasketCommand : ICommandWithReturnAndParamAsync<IQueryable<Product>, int>
    {
    }
}