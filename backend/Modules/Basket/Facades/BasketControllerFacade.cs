using StoreBackend.Interfaces;

namespace StoreBackend.Facade
{
    public class BasketControllerFacade
    {
        public BasketControllerFacade(
            IQueryProductsInBasketCommand queryProductsInBasketCommand,
            IQueryProductIdsInBasketCommand queryProductIdsInBasketCommand)
        {
            QueryProductsInBasketCommand = queryProductsInBasketCommand;
            QueryProductIdsInBasketCommand = queryProductIdsInBasketCommand;
        }
        public IQueryProductsInBasketCommand QueryProductsInBasketCommand { get; }
        public IQueryProductIdsInBasketCommand QueryProductIdsInBasketCommand { get; }
    }
}