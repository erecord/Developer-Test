using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Factories;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Repositories;
using Xunit;

namespace StoreBackend.Tests
{
    public class AddProductToBasketCommandTests : BaseTest
    {
        private IBasketProductFactory _basketProductFactory;
        private IBasketProductRepository _basketProductRepository;
        private IBasketRepository _basketRepository;
        private IProductRepository _productRepository;
        private IAddProductsToBasketCommand _addProductsToBasketCommand;
        private const int BasketId = 1;

        public AddProductToBasketCommandTests()
        {
            _basketProductFactory = new BasketProductFactory();
        }


        [Fact]
        public async void AddProductsToBasketCommand_WhenSingleProductIsAdded_BasketContainsTheSingleProduct()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToAdd = 1;
                await _addProductsToBasketCommand.Execute((BasketId, new[] { productIdToAdd }));

                var basketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(BasketId))).ToListAsync();
                var productIdsInBasket = basketProductsList.Select(bp => bp.ProductId);

                productIdsInBasket.Should().HaveCount(1);
                productIdsInBasket.Should().Contain(productIdToAdd);
            }
        }

        [Fact]
        public async void AddProductsToBasketCommand_WhenMultipleProductsAreAdded_BasketContainsTheMultipleProducts()
        {
            using (var context = InitAndGetDbContext())
            {
                var productsToAdd = new[] { 1, 2, 3 };
                await _addProductsToBasketCommand.Execute((BasketId, productsToAdd));

                var basketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(BasketId))).ToListAsync();
                var productIdsInBasket = basketProductsList.Select(bp => bp.ProductId);

                productIdsInBasket.Should().BeEquivalentTo(productsToAdd);
            }
        }

        [Fact]
        public async Task AddProductsToBasketCommand_WhenProductDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentProductId = -1;
                Func<Task> act = () => _addProductsToBasketCommand.Execute((BasketId, new[] { nonexistentProductId }));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Fact]
        public async Task AddProductsToBasketCommand_WhenBasketDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistentBasketId = -1;
                Func<Task> act = () => _addProductsToBasketCommand.Execute((nonexistentBasketId, new[] { 1 }));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user = new User
            {
                Id = 1,
                Email = "test@gmail.com",
                Username = "Test",
                Password = "password1"
            };

            var basket = new Basket
            {
                Id = BasketId,
                userId = 1
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

            context.User.Add(user);
            context.Product.AddRange(product1, product2, product3);
            context.Basket.Add(basket);

            context.SaveChanges();

            _basketProductRepository = new BasketProductRepository(context);
            _basketRepository = new BasketRepository(context);
            _productRepository = new ProductRepository(context);
            _addProductsToBasketCommand = new AddProductsToBasketCommand(_basketProductRepository, _basketRepository, _productRepository, _basketProductFactory);
            return context;
        }
    }
}