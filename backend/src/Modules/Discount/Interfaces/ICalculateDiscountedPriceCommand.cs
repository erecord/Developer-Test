using StoreBackend.Common.Interfaces;

namespace StoreBackend.Interfaces
{
    public interface ICalculateDiscountedPriceCommand : ICommandWithReturnAndParam<decimal, (decimal originalPrice, decimal discountPercentage)>
    {

    }
}