using System;
using FakeItEasy;
using FluentAssertions;
using TravelAgency.ImperativeCode;
using TravelAgency.ImperativeCode.Discounts;
using Xunit;

namespace Tests.ImperativeCode {
    public class LastMinuteDiscounterTests {
        [Fact]
        public void should_discount_by_last_minute() {
            //arrange
            var price       = 200m;
            var currentDate = new DateTimeOffset(2021, 5, 20, 12, 11, 10, TimeSpan.Zero);
            var travelDate  = new DateTimeOffset(2021, 6, 1, 10, 0, 0, TimeSpan.Zero);

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.GetUtcNow()).Returns(currentDate).Once();
            var sut = new LastMinuteDiscounter(dateTimeProvider);

            var expected = 160m;

            //act
            var result = sut.Discount(price, travelDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_last_minute() {
            //arrange
            var price       = 123m;
            var currentDate = new DateTimeOffset(2021, 3, 4, 12, 11, 10, TimeSpan.Zero);
            var travelDate  = new DateTimeOffset(2022, 12, 1, 10, 0, 0, TimeSpan.Zero);

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.GetUtcNow()).Returns(currentDate).Once();
            var sut = new LastMinuteDiscounter(dateTimeProvider);

            var expected = 123m;

            //act
            var result = sut.Discount(price, travelDate);

            //assert
            result.Should().Be(expected);
        }
    }
}