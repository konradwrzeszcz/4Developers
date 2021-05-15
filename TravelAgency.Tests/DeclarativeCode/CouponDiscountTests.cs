using System;
using FluentAssertions;
using TravelAgency.DeclarativeCode.Domain;
using Xunit;

namespace Tests.DeclarativeCode {
    public class CouponDiscountTests {
        [Fact]
        public void should_discount_by_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "CHEAPER_TRAVEL_2021";
            var currentDate = new DateTimeOffset(2021, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var expected = 80m;
            //act
            var result = price.CalculateCouponDiscount(couponCode, currentDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_wrong_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "wrong_coupon";
            var currentDate = new DateTimeOffset(2021, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var expected = 100m;

            //act
            var result = price.CalculateCouponDiscount(couponCode, currentDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_expired_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "CHEAPER_TRAVEL_2021";
            var currentDate = new DateTimeOffset(2030, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var expected = 100m;

            //act
            var result = price.CalculateCouponDiscount(couponCode, currentDate);

            //assert
            result.Should().Be(expected);
        }
    }
}