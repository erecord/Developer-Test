using StoreBackend.Interfaces;

namespace StoreBackend.Facade
{


    public class BasketControllerFacade : IBasketControllerFacade
    {
        public BasketControllerFacade(
            IQueryProductsInBasketCommand queryProductsInBasketCommand,
            IQueryProductIdsInBasketCommand queryProductIdsInBasketCommand,
            IRemoveProductsFromBasketCommand removeProductsFromBasketCommand,
            IAddProductsToBasketCommand addProductIdsToBasketCommand,
            IBasketDiscountService basketDiscountService,
            IQueryTotalCostOfBasketCommand queryTotalCostOfBasketCommand
        )
        {
            QueryProductsInBasketCommand = queryProductsInBasketCommand;
            QueryProductIdsInBasketCommand = queryProductIdsInBasketCommand;
            RemoveProductsFromBasketCommand = removeProductsFromBasketCommand;
            AddProductsToBasketCommand = addProductIdsToBasketCommand;
            BasketDiscountService = basketDiscountService;
            QueryTotalCostOfBasketCommand = queryTotalCostOfBasketCommand;
        }
        public IQueryProductsInBasketCommand QueryProductsInBasketCommand { get; }
        public IQueryProductIdsInBasketCommand QueryProductIdsInBasketCommand { get; }
        public IRemoveProductsFromBasketCommand RemoveProductsFromBasketCommand { get; }
        public IAddProductsToBasketCommand AddProductsToBasketCommand { get; }
        public IBasketDiscountService BasketDiscountService { get; }
        public IQueryTotalCostOfBasketCommand QueryTotalCostOfBasketCommand { get; }
    }
}