using System;
using FakeItEasy;
using FluentAssertions;
using TravelAgency.ImperativeCode;
using TravelAgency.ImperativeCode.Discounts;
using Xunit;

namespace Tests.ImperativeCode {
    public class CouponDiscounterTests {
        [Fact]
        public void should_discount_by_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "CHEAPER_TRAVEL_2021";
            var currentDate = new DateTimeOffset(2021, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.GetUtcNow()).Returns(currentDate).Once();

            var sut = new CouponDiscounter(dateTimeProvider);

            var expected = 80m;

            //act
            var result = sut.Discount(price, couponCode);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_wrong_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "wrong_coupon";
            var currentDate = new DateTimeOffset(2021, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.GetUtcNow()).Returns(currentDate).Once();

            var sut = new CouponDiscounter(dateTimeProvider);

            var expected = 100m;

            //act
            var result = sut.Discount(price, couponCode);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_expired_coupon() {
            //arrange
            var price       = 100m;
            var couponCode  = "CHEAPER_TRAVEL_2021";
            var currentDate = new DateTimeOffset(2030, 5, 3, 12, 11, 10, TimeSpan.Zero);

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.GetUtcNow()).Returns(currentDate).Once();

            var sut = new CouponDiscounter(dateTimeProvider);

            var expected = 100m;

            //act
            var result = sut.Discount(price, couponCode);

            //assert
            result.Should().Be(expected);
        }
    }
}