using StoreBackend.Interfaces;

namespace StoreBackend.Facade
{


    public class BasketControllerFacade : IBasketControllerFacade
    {
        public BasketControllerFacade(
            IQueryProductsInBasketCommand queryProductsInBasketCommand,
            IQueryProductIdsInBasketCommand queryProductIdsInBasketCommand,
            IRemoveProductsFromBasketCommand removeProductsFromBasketCommand,
            IAddProductsToBasketCommand addProductIdsToBasketCommand
        )
        {
            QueryProductsInBasketCommand = queryProductsInBasketCommand;
            QueryProductIdsInBasketCommand = queryProductIdsInBasketCommand;
            RemoveProductsFromBasketCommand = removeProductsFromBasketCommand;
            AddProductsToBasketCommand = addProductIdsToBasketCommand;
        }
        public IQueryProductsInBasketCommand QueryProductsInBasketCommand { get; }
        public IQueryProductIdsInBasketCommand QueryProductIdsInBasketCommand { get; }
        public IRemoveProductsFromBasketCommand RemoveProductsFromBasketCommand { get; }
        public IAddProductsToBasketCommand AddProductsToBasketCommand { get; }
    }
}