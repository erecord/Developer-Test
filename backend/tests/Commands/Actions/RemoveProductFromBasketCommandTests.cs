using System;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Extensions;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;
using StoreBackend.Tests.Factories;
using Xunit;

namespace StoreBackend.Tests
{
    public class RemoveProductFromBasketCommandTests : BaseTest
    {
        private BasketProductRepository _basketProductRepository;
        private IRemoveProductsFromBasketCommand _SUT;
        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        [Fact]
        public async void Execute_WhenSingleProductIsRemoved_BasketDoesNotContainTheSingleProduct()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = 2;
                await _SUT.Execute((BasketId, new[] { productIdToRemove }));

                var productIdsInBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(BasketId);

                productIdsInBasket.Should().Contain(new[] { 1, 3 });
                productIdsInBasket.Should().NotContain(2);
            }
        }

        [Fact]
        public async void Execute_WhenMultipleProductsAreRemoved_BasketDoesNotContainTheRemovedProducts()
        {
            using (var context = InitAndGetDbContext())
            {
                var productsToRemove = new[] { 2, 3 };
                await _SUT.Execute((BasketId, productsToRemove));

                var productIdsInBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(BasketId);

                productIdsInBasket.Should().Contain(1);
                productIdsInBasket.Should().NotContain(productsToRemove);
            }
        }

        [Fact]
        public async void Execute_WhenProductIsRemoved_ProductIsRemovedFromOnlyTheCorrectBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = new[] { 1 };
                await _SUT.Execute((BasketId, productIdToRemove));

                var productIdsInBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(BasketId);
                var productIdsInSecondBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(SecondBasketId);

                productIdsInBasket.Should().Contain(new[] { 2, 3 });
                productIdsInBasket.Should().NotContain(productIdToRemove);

                productIdsInSecondBasket.Should().Contain(productIdToRemove);
            }
        }

        [Fact]
        public async void Execute_WhenProductIdIsNotInBasket_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = new[] { 4 };
                Func<Task> act = () => _SUT.Execute((BasketId, productIdToRemove));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Fact]
        public async void Execute_WhenBasketIdDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistingBasketId = -1;
                Func<Task> act = () => _SUT.Execute((nonexistingBasketId, new[] { 1 }));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user1 = TestUserFactory.CreateRandomUser(1);
            var user2 = TestUserFactory.CreateRandomUser(2);

            var products = new[] {
                TestProductFactory.CreateProduct(1, 7.99M),
                TestProductFactory.CreateProduct(2, 300M),
                TestProductFactory.CreateProduct(3, 5M),
            };

            var (basket1, basketProductsList1) = TestBasketFactory.CreateBasketWithExistingProducts(BasketId, user1.Id, products);
            var (basket2, basketProductsList2) = TestBasketFactory.CreateBasketWithExistingProducts(SecondBasketId, user2.Id, new[] { products[0] });

            context.User.AddRange(user1, user2);
            context.Product.AddRange(products);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProductsList1);
            context.BasketProducts.AddRange(basketProductsList2);

            context.SaveChanges();

            _basketProductRepository = new BasketProductRepository(context);
            _SUT = new RemoveProductsFromBasketCommand(_basketProductRepository);
            return context;
        }
    }
}