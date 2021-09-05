using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Exceptions;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;
using StoreBackend.Tests.Factories;
using Xunit;

namespace StoreBackend.Tests
{
    public class QueryProductIdsInBasketCommandTests : BaseTest
    {
        private IQueryProductIdsInBasketCommand _SUT;
        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        [Fact]
        public async void Execute_WhenProductsAreInBasket_ReturnsTheProductIdsOfAllProductsInBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdsInBasketQuery = await _SUT.Execute(BasketId);

                var expectedProductIds = new[] { 1, 2, 3 };
                productIdsInBasketQuery.Should().BeEquivalentTo(expectedProductIds);
            }
        }

        [Fact]
        public async void Execute_WhenBasketIdDoesNotExist_ThrowsBasketNotFoundException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentBasketId = -1;
                Func<Task> act = () => _SUT.Execute(nonexistentBasketId);

                await act.Should().ThrowAsync<BasketNotFoundException>();
            }
        }

        [Fact]
        public async void Execute_WhenBasketIsEmpty_ReturnsEmptyQueryable()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdsInBasket = await _SUT.Execute(SecondBasketId);

                productIdsInBasket.Should().HaveCount(0);
                productIdsInBasket.Should().BeAssignableTo<IQueryable<int>>();
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
            var basket2 = TestBasketFactory.CreateEmptyBasket(SecondBasketId, user2.Id);

            context.User.AddRange(user1, user2);
            context.Product.AddRange(products);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProductsList1);

            context.SaveChanges();

            var basketProductRepository = new BasketProductRepository(context);
            var basketRepository = new BasketRepository(context);
            _SUT = new QueryProductIdsInBasketCommand(basketProductRepository, basketRepository);
            return context;
        }
    }
}