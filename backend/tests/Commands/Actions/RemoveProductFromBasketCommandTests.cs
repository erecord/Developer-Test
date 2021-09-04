using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Repositories;
using Xunit;

namespace StoreBackend.Tests
{
    public class RemoveProductFromBasketCommandTests : BaseTest
    {
        private BasketProductRepository _basketProductRepository;
        private IRemoveProductsFromBasketCommand _removeProductsFromBasketCommand;
        private const int BasketId = 1;
        private const int SecondBasketId = 2;

        [Fact]
        public async void RemoveProductsFromBasketCommand_WhenSingleProductIsRemoved_BasketDoesNotContainTheSingleProduct()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = 2;
                await _removeProductsFromBasketCommand.Execute((BasketId, new[] { productIdToRemove }));

                var basketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(BasketId))).ToListAsync();
                var productIdsInBasket = basketProductsList.Select(bp => bp.ProductId);

                productIdsInBasket.Should().Contain(new[] { 1, 3 });
                productIdsInBasket.Should().NotContain(2);
            }
        }

        [Fact]
        public async void RemoveProductsFromBasketCommand_WhenMultipleProductsAreRemoved_BasketDoesNotContainTheRemovedProducts()
        {
            using (var context = InitAndGetDbContext())
            {
                var productsToRemove = new[] { 2, 3 };
                await _removeProductsFromBasketCommand.Execute((BasketId, productsToRemove));

                var basketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(BasketId))).ToListAsync();
                var productIdsInBasket = basketProductsList.Select(bp => bp.ProductId);

                productIdsInBasket.Should().Contain(1);
                productIdsInBasket.Should().NotContain(productsToRemove);
            }
        }

        [Fact]
        public async void RemoveProductsFromBasketCommand_WhenProductIsRemoved_ProductIsRemovedFromOnlyTheCorrectBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = new[] { 1 };
                await _removeProductsFromBasketCommand.Execute((BasketId, productIdToRemove));

                var basketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(BasketId))).ToListAsync();
                var productIdsInBasket = basketProductsList.Select(bp => bp.ProductId);

                var secondBasketProductsList = await (await _basketProductRepository.WhereAsync(bp => bp.BasketId.Equals(SecondBasketId))).ToListAsync();
                var productIdsInSecondBasket = secondBasketProductsList.Select(bp => bp.ProductId);

                productIdsInBasket.Should().Contain(new[] { 2, 3 });
                productIdsInBasket.Should().NotContain(productIdToRemove);

                productIdsInSecondBasket.Should().Contain(productIdToRemove);
            }
        }

        [Fact]
        public async void RemoveProductsFromBasketCommand_WhenProductIdIsNotInBasket_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var productIdToRemove = new[] { 4 };
                Func<Task> act = () => _removeProductsFromBasketCommand.Execute((BasketId, productIdToRemove));

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Fact]
        public async void RemoveProductsFromBasketCommand_WhenBasketIdDoesNotExist_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistingBasketId = -1;
                Func<Task> act = () => _removeProductsFromBasketCommand.Execute((nonexistingBasketId, new[] { 1 }));

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
                Id = 1,
                userId = 1
            };

            var basket2 = new Basket
            {
                Id = 2,
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

            var basketProduct4 = new BasketProduct
            {
                BasketId = SecondBasketId,
                ProductId = 1
            };

            context.User.Add(user1);
            context.User.Add(user2);
            context.Product.AddRange(product1, product2, product3);
            context.Basket.Add(basket1);
            context.Basket.Add(basket2);
            context.BasketProducts.AddRange(basketProduct1, basketProduct2, basketProduct3, basketProduct4);

            context.SaveChanges();

            _basketProductRepository = new BasketProductRepository(context);
            _removeProductsFromBasketCommand = new RemoveProductsFromBasketCommand(_basketProductRepository);
            return context;
        }
    }
}