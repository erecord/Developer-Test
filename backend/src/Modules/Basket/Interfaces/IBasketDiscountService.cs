using System.Threading.Tasks;

namespace StoreBackend.Interfaces
{
    public interface IBasketDiscountService
    {
        Task SetDiscountOnBasketAsync(int basketId, string discountCode);
        Task<decimal> QueryBasketTotalCostAfterDiscountAsync(int basketId);
    }
}