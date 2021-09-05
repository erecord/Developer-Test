using System;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.DbContexts;
using StoreBackend.Exceptions;
using StoreBackend.Interfaces;
using StoreBackend.Repositories;
using StoreBackend.Services;
using StoreBackend.Tests.Factories;
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
        public const decimal Discount2Percentage = 10;

        public const decimal Product1Price = 7.99M;
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
                var (basketCostBeforeDiscount, basketCostAfterDiscount) = await _SUT.QueryBasketTotalCostWithDiscountAsync(SecondBasketId);

                var expectedBasketCost = Product1Price;
                var expectedBasketCostAfterDiscount = _calculateDiscountedPriceCommand.Execute((expectedBasketCost, Discount1Percentage));

                basketCostBeforeDiscount.Should().Be(expectedBasketCost);
                basketCostAfterDiscount.Should().Be(expectedBasketCostAfterDiscount);
            }
        }

        [Fact]
        public async void QueryBasketTotalCostWithDiscount_WhenBasketDoesNotHaveDiscountCodeSet_ThrowsInvalidOperationException()
        {
            using (var context = InitAndGetDbContext())
            {
                Func<Task> act = () => _SUT.QueryBasketTotalCostWithDiscountAsync(BasketId);

                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        private StoreDbContext InitAndGetDbContext()
        {
            var context = GetDbContext();

            var user1 = TestUserFactory.CreateRandomUser(1);
            var user2 = TestUserFactory.CreateRandomUser(2);

            var products = new[] {
                TestProductFactory.CreateProduct(1, Product1Price),
                TestProductFactory.CreateProduct(2, 300M),
            };

            var (basket1, basketProductsList1) = TestBasketFactory.CreateBasketWithExistingProducts(BasketId, user1.Id, products);
            var (basket2, basketProductsList2) = TestBasketFactory.CreateBasketWithExistingProducts(SecondBasketId, user2.Id, new[] { products[0] });
            basket2.discountId = Discount1Id;

            var discount1 = TestDiscountFactory.CreateDiscount(Discount1Id, Discount1Code, Discount1Percentage);
            var discount2 = TestDiscountFactory.CreateDiscount(Discount2Id, Discount2Code, Discount2Percentage);

            context.User.AddRange(user1, user2);
            context.Product.AddRange(products);
            context.Basket.AddRange(basket1, basket2);
            context.BasketProducts.AddRange(basketProductsList1);
            context.BasketProducts.AddRange(basketProductsList2);
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