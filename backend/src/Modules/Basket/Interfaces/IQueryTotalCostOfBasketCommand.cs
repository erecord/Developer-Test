using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IQueryTotalCostOfBasketCommand : ICommandWithReturnAndParam<decimal, int>
    {
    }
}