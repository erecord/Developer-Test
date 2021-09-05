using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface IQueryTotalCostOfBasketCommand : ICommandWithReturnAndParamAsync<decimal, int>
    {
    }
}