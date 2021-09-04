using System;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Exceptions;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Repositories;
using StoreBackend.Services;
using Xunit;

namespace StoreBackend.Tests
{
    public class BasketDiscountServiceTests : BaseTest
    {
        public const int BasketId = 1;
        public const int SecondBasketId = 2;
        public const int Discount1Id = 1;
        public const string Discount1Code = "SALE20";
        public const decimal Discount1Percentage = 20;
        public const int Discount2Id = 2;
        public const string Discount2Code = "SALE10";

        public const decimal Product1Price = 7.99M;
        public const decimal Product2Price = 300M;
        private IBasketDiscountService _SUT;
        private ICalculateDiscountedPriceCommand _calculateDiscountedPriceCommand = new CalculateDiscountedPriceCommand();

        [Fact]
        public async void SetDiscount_WhenDiscountIdExists_ThenDiscountIsLinkedToTheBasket()
        {
            using (var context = InitAndGetDbContext())
            {
                await _SUT.SetDiscountOnBasketAsync(BasketId, Discount1Code);

                var basket = await context.Basket.FindAsync(BasketId);
                basket.discountId.Should().Be(Discount1Id);
            }
        }

        [Fact]
        public async void SetDiscount_WhenDiscountIdDoesNotExists_ThrowsDiscountCodeNotFoundException()
        {
            using (var context = InitAndGetDbContext())
            {
                Func<Task> act = () => _SUT.SetDiscountOnBasketAsync(BasketId, "InvalidDiscountCode");

                await act.Should().ThrowAsync<DiscountCodeNotFoundException>();
            }
        }

        [Fact]
        public async void SetDiscount_WhenNewDiscountIsUsedOnBasketThatHasDiscountCode_ThenDiscountCodeIsUpdatedWithNewCode()
        {
            using (var context = InitAndGetDbContext())
            {
                await _SUT.SetDiscountOnBasketAsync(SecondBasketId, Discount2Code);

                var basket = await context.Basket.FindAsync(SecondBasketId);
                basket.discountId.Should().Be(Discount2Id);
            }
        }

        [Fact]
        public async void SetDiscount_WhenBasketIdDoesNotExists_ThrowsBasketNotFoundException()
        {
            using (var context = InitAndGetDbContext())
            {
                var nonexistingBasketId = -1;
                Func<Task> act = () => _SUT.SetDiscountOnBasketAsync(nonexistingBasketId, Discount1Code);

                await act.Should().ThrowAsync<BasketNotFoundException>();
            }
        }

        [Fact]
        public async void QueryBasketTotalCostWithDiscount_WhenBasketHasDiscountCodeSet_ReturnsTotalCostOfBasketWithDiscountApplied()
        {
            using (var context = InitAndGetDbContext())
            {
                var (_, basketCostAfterDiscount) = await _SUT.QueryBasketTotalCostWithDiscountAsync(SecondBasketId);

                var expectedBasketCost = Product1Price;
                var expectedBasketCostAfterDiscount = _calculateDiscountedPriceCommand.Execute((expectedBasketCost, Discount1Percentage));

                basketCostAfterDiscount.Should().Be(expectedBasketCostAfterDiscount);
            }
        }

        [Fact]
        public async void QueryBasketTotalCostWithDiscount_WhenBasketDoesNotHaveDiscountCodeSet_ThrowsDiscountCodeNotFoundException()
        {
            using (var context = InitAndGetDbContext())
            {
                Func<Task> act = () => _SUT.QueryBasketTotalCostWithDiscountAsync(BasketId);

                await act.Should().ThrowAsync<DiscountCodeNotFoundException>();
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
                userId = 2,
                discountId = Discount1Id
            };

            var product1 = new Product
            {
                Id = 1,
                Name = "Book",
                Price = Product1Price
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Computer",
                Price = Product2Price
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
                BasketId = SecondBasketId,
                ProductId = 1
            };

            var discount1 = new Discount
            {
                Id = Discount1Id,
                Code = Discount1Code,
                Percentage = Discount1Percentage,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            var discount2 = new Discount
            {
                Id = Discount2Id,
                Code = Discount2Code,
                Percentage = 10,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            context.User.AddRange(user1, user2);
            context.Product.AddRange(product1, product2);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProduct1, basketProduct2, basketProduct3);
            context.Discount.AddRange(discount1, discount2);

            context.SaveChanges();

            var basketRepository = new BasketRepository(context);
            var discountRepository = new DiscountRepository(context);
            var productRepository = new ProductRepository(context);
            var basketProductRepository = new BasketProductRepository(context);

            var queryProductIdsInBasketCommand = new QueryProductIdsInBasketCommand(basketProductRepository, basketRepository);
            var queryProductsInBasketCommand = new QueryProductsInBasketCommand(productRepository, queryProductIdsInBasketCommand);
            var queryTotalCostOfBasketCommand = new QueryTotalCostOfBasketCommand(queryProductsInBasketCommand, productRepository);
            _SUT = new BasketDiscountService(basketRepository, discountRepository, queryTotalCostOfBasketCommand, _calculateDiscountedPriceCommand);

            return context;
        }
    }
}