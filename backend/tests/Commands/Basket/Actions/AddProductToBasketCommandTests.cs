using System;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Extensions;
using StoreBackend.Factories;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;
using StoreBackend.Tests.Factories;
using Xunit;

namespace StoreBackend.Tests
{
    public class AddProductToBasketCommandTests : BaseTest
    {
        private IBasketProductFactory _basketProductFactory = new BasketProductFactory();
        private IBasketProductRepository _basketProductRepository;
        private IBasketRepository _basketRepository;
        private IProductRepository _productRepository;
        private IAddProductsToBasketCommand _SUT;
        private const int BasketId = 1;

        [Fact]
        public async void Execute_WhenSingleProductIsAdded_BasketContainsTheSingleProduct()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToAdd = 1;
                await _SUT.Execute((BasketId, new[] { productIdToAdd }));

                var productIdsInBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(BasketId);

                productIdsInBasket.Should().HaveCount(1);
                productIdsInBasket.Should().Contain(productIdToAdd);
            }
        }

        [Fact]
        public async void Execute_WhenMultipleProductsAreAdded_BasketContainsTheMultipleProducts()
        {
            using (var context = InitAndGetDbContext())
            {
                var productsToAdd = new[] { 1, 2, 3 };
                await _SUT.Execute((BasketId, productsToAdd));

                var productIdsInBasket = await _basketProductRepository.QueryProductIdsInBasketAsync(BasketId);

                productIdsInBasket.Should().BeEquivalentTo(productsToAdd);
            }
        }

        [Fact]
        public async Task Execute_WhenProductDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentProductId = -1;
                Func<Task> act = () => _SUT.Execute((BasketId, new[] { nonexistentProductId }));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Fact]
        public async Task Execute_WhenBasketDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentBasketId = -1;
                Func<Task> act = () => _SUT.Execute((nonexistentBasketId, new[] { 1 }));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user = TestUserFactory.CreateRandomUser(1);

            var products = new[] {
                TestProductFactory.CreateProduct(1, 7.99M),
                TestProductFactory.CreateProduct(2, 300M),
                TestProductFactory.CreateProduct(3, 5M)
            };

            var basket = TestBasketFactory.CreateEmptyBasket(BasketId, user.Id);

            context.User.Add(user);
            context.Product.AddRange(products);
            context.Basket.Add(basket);

            context.SaveChanges();

            _basketProductRepository = new BasketProductRepository(context);
            _basketRepository = new BasketRepository(context);
            _productRepository = new ProductRepository(context);
            _SUT = new AddProductsToBasketCommand(_basketProductRepository, _basketRepository, _productRepository, _basketProductFactory);
            return context;
        }
    }
}