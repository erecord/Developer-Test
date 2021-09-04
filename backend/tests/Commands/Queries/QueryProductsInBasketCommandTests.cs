using System;
using System.Collections.Generic;
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
    public class QueryProductsInBasketCommandTests : BaseTest
    {
        private IQueryProductsInBasketCommand _SUT;

        private List<Product> _testProducts = new List<Product>();
        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        [Fact]
        public async void Execute_WhenProductsAreInBasket_ReturnsAllProductsInBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                var productsInBasketQueryable = await _SUT.Execute(BasketId);

                productsInBasketQueryable.Should().BeEquivalentTo(_testProducts);
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
                var productInBasket = await _SUT.Execute(SecondBasketId);

                productInBasket.Should().HaveCount(0);
                productInBasket.Should().BeAssignableTo<IQueryable<Product>>();
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

            _testProducts.AddRange(new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Book",
                    Price = 7.99M
                },
                new Product
                {
                    Id = 2,
                    Name = "Computer",
                    Price = 300M
                },
                new Product
                {
                    Id = 3,
                    Name = "Hat",
                    Price = 5M
                }
            });


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
            context.Product.AddRange(_testProducts);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProduct1, basketProduct2, basketProduct3);

            context.SaveChanges();

            var basketProductRepository = new BasketProductRepository(context);
            var basketRepository = new BasketRepository(context);
            var productRepository = new ProductRepository(context);
            var queryProductIdsInBasketCommand = new QueryProductIdsInBasketCommand(basketProductRepository, basketRepository);
            _SUT = new QueryProductsInBasketCommand(productRepository, queryProductIdsInBasketCommand);
            return context;
        }
    }
}