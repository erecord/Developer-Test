using System;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Repositories;
using Xunit;

namespace StoreBackend.Tests
{
    public class QueryTotalCostOfBasketCommandTests : BaseTest
    {

        private IQueryTotalCostOfBasketCommand _SUT;

        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        private decimal product1Cost = 7.99M;
        private decimal product2Cost = 300M;
        private decimal product3Cost = 5M;


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
        public async void Execute_WhenBasketIdDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistingBasketId = -1;
                Func<Task> act = () => _SUT.Execute(nonexistingBasketId);

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user1 = new User
            {
                Id = 1,
                Email = "test@gmail.com",
                Username = "Test",
                Password = "password1"
            };

            var user2 = new User
            {
                Id = 2,
                Email = "test2@gmail.com",
                Username = "Test2",
                Password = "password2"
            };

            var basket1 = new Basket
            {
                Id = BasketId,
                userId = 1
            };

            var basket2 = new Basket
            {
                Id = SecondBasketId,
                userId = 2
            };

            var product1 = new Product
            {
                Id = 1,
                Name = "Book",
                Price = product1Cost
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Computer",
                Price = product2Cost
            };

            var product3 = new Product
            {
                Id = 3,
                Name = "Hat",
                Price = product3Cost
            };

            var basketProduct1 = new BasketProduct
            {
                BasketId = BasketId,
                ProductId = 1
            };

            var basketProduct2 = new BasketProduct
            {
                BasketId = BasketId,
                ProductId = 2
            };

            var basketProduct3 = new BasketProduct
            {
                BasketId = BasketId,
                ProductId = 3
            };

            context.User.Add(user1);
            context.Product.AddRange(product1, product2, product3);
            context.Basket.Add(basket1);
            context.Basket.Add(basket2);
            context.BasketProducts.AddRange(basketProduct1, basketProduct2, basketProduct3);

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