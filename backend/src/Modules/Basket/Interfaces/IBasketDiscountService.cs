using System.Threading.Tasks;

namespace StoreBackend.Interfaces
{
    public interface IBasketDiscountService
    {
        Task SetDiscountOnBasketAsync(int basketId, string discountCode);
        Task<(decimal basketCostBeforeDiscount, decimal basketCostAfterDiscount)> QueryBasketTotalCostWithDiscountAsync(int basketId);
    }
}