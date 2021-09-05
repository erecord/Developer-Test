using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Repositories;
using StoreBackend.Tests.Factories;
using Xunit;

namespace StoreBackend.Tests
{
    public class QueryProductsInBasketCommandTests : BaseTest
    {
        private IQueryProductsInBasketCommand _SUT;

        private Product[] _testProducts;
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

            var user1 = TestUserFactory.CreateRandomUser(1);
            var user2 = TestUserFactory.CreateRandomUser(2);

            _testProducts = new[] {
                TestProductFactory.CreateProduct(1, 7.99M),
                TestProductFactory.CreateProduct(2, 300M),
                TestProductFactory.CreateProduct(3, 5M),
            };

            var (basket1, basketProductsList1) = TestBasketFactory.CreateBasketWithExistingProducts(BasketId, user1.Id, _testProducts);
            var basket2 = TestBasketFactory.CreateEmptyBasket(SecondBasketId, user2.Id);

            context.User.AddRange(user1, user2);
            context.Product.AddRange(_testProducts);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProductsList1);

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