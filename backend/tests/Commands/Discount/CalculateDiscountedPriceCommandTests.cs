using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using StoreBackend.Commands;
using StoreBackend.Interfaces;
using Xunit;

namespace StoreBackend.Tests
{
    public class CalculateDiscountedPriceCommandTests : BaseTest
    {
        private ICalculateDiscountedPriceCommand _SUT;

        public CalculateDiscountedPriceCommandTests()
        {
            _SUT = new CalculateDiscountedPriceCommand();
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                    new object[] { 100M, 10M },
                    new object[] { 0M, 10M },
                    new object[] { 100M, 0M },
                    new object[] { 100M, 100M },
                    new object[] { 0M, 0M },
            };

        public static IEnumerable<object[]> OutOfBoundsData =>
            new List<object[]>
            {
                    new object[] { 100M, -1M },
                    new object[] { 100M, 101M },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Execute_WithPriceAndDiscountPercentage_ReturnsDiscountedPrice(decimal originalPrice, decimal discountPercentage)
        {
            var discountedPrice = _SUT.Execute((originalPrice, discountPercentage));

            var scaledDiscountPercentage = discountPercentage / 100;
            var expectedDiscountSavings = originalPrice * scaledDiscountPercentage;
            var expectedDiscountedPrice = originalPrice - expectedDiscountSavings;

            discountedPrice.Should().Be(expectedDiscountedPrice);
        }

        [Theory]
        [MemberData(nameof(OutOfBoundsData))]
        public void Execute_WhenDiscountPercentageOutOfBounds_ThrowsInvalidOperationException(decimal originalPrice, decimal discountPercentage)
        {
            Action act = () => _SUT.Execute((originalPrice, discountPercentage));

            act.Should().Throw<InvalidOperationException>();
        }
    }
}