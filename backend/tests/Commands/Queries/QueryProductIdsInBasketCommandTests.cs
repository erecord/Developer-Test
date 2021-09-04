using System;
using System.Linq;
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
        public async void Execute_WhenBasketIdDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentBasketId = -1;
                Func<Task> act = () => _SUT.Execute(nonexistentBasketId);

                await act.Should().ThrowAsync<InvalidOperationException>();
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
                Price = 7.99M
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Computer",
                Price = 300M
            };

            var product3 = new Product
            {
                Id = 3,
                Name = "Hat",
                Price = 5M
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
            context.User.Add(user2);
            context.Product.AddRange(product1, product2, product3);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProduct1, basketProduct2, basketProduct3);

            context.SaveChanges();

            var basketProductRepository = new BasketProductRepository(context);
            var basketRepository = new BasketRepository(context);
            _SUT = new QueryProductIdsInBasketCommand(basketProductRepository, basketRepository);
            return context;
        }
    }
}