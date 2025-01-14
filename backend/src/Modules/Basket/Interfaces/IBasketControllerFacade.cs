namespace StoreBackend.Interfaces
{
    public interface IBasketControllerFacade
    {
        IQueryProductsInBasketCommand QueryProductsInBasketCommand { get; }
        IQueryProductIdsInBasketCommand QueryProductIdsInBasketCommand { get; }
        IRemoveProductsFromBasketCommand RemoveProductsFromBasketCommand { get; }
        IAddProductsToBasketCommand AddProductsToBasketCommand { get; }
        IBasketDiscountService BasketDiscountService { get; }
        IQueryTotalCostOfBasketCommand QueryTotalCostOfBasketCommand { get; }
    }
}