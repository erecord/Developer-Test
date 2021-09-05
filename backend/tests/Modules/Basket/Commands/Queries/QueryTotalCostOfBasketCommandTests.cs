using System;
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
    public class QueryTotalCostOfBasketCommandTests : BaseTest
    {

        private IQueryTotalCostOfBasketCommand _SUT;

        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        private const decimal product1Cost = 7.99M;
        private const decimal product2Cost = 300M;
        private const decimal product3Cost = 5M;


        [Fact]
        public async void Execute_WhenProductsAreInBasket_BasketTotalEqualsCostOfAllProductsInBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                var totalCostOfBasket = await _SUT.Execute(BasketId);

                var expectedTotalCostOfBasket = product1Cost + product2Cost + product3Cost;
                totalCostOfBasket.Should().Be(expectedTotalCostOfBasket);
            }
        }

        [Fact]
        public async void Execute_WhenNoProductsAreInBasket_BasketTotalEqualsZero()
        {
            using (var context = InitAndGetDbContext())
            {
                var totalCostOfBasket = await _SUT.Execute(SecondBasketId);

                var expectedTotalCostOfBasket = 0;
                totalCostOfBasket.Should().Be(expectedTotalCostOfBasket);
            }
        }

        [Fact]
        public async void Execute_WhenBasketIdDoesNotExist_ThrowsBasketNotFoundException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistingBasketId = -1;
                Func<Task> act = () => _SUT.Execute(nonexistingBasketId);

                await act.Should().ThrowAsync<BasketNotFoundException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user1 = TestUserFactory.CreateRandomUser(1);
            var user2 = TestUserFactory.CreateRandomUser(2);

            var products = new[] {
                TestProductFactory.CreateProduct(1, product1Cost),
                TestProductFactory.CreateProduct(2, product2Cost),
                TestProductFactory.CreateProduct(3, product3Cost),
            };

            var (basket1, basketProductsList1) = TestBasketFactory.CreateBasketWithExistingProducts(BasketId, user1.Id, products);
            var basket2 = TestBasketFactory.CreateEmptyBasket(SecondBasketId, user2.Id);

            context.User.Add(user1);
            context.Product.AddRange(products);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProductsList1);

            context.SaveChanges();

            var productRepository = new ProductRepository(context);
            var basketProductRepository = new BasketProductRepository(context);
            var basketRepository = new BasketRepository(context);
            var queryProductIdsInBasketCommand = new QueryProductIdsInBasketCommand(basketProductRepository, basketRepository);
            var queryProductsInBasketCommand = new QueryProductsInBasketCommand(productRepository, queryProductIdsInBasketCommand);
            _SUT = new QueryTotalCostOfBasketCommand(queryProductsInBasketCommand, productRepository);
            return context;
        }
    }
}