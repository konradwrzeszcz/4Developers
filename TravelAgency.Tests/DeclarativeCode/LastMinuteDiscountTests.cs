using System;
using FluentAssertions;
using TravelAgency.DeclarativeCode.Domain;
using Xunit;

namespace Tests.DeclarativeCode {
    public class LastMinuteDiscountTests {
        [Fact]
        public void should_discount_by_last_minute() {
            //arrange
            var price       = 200m;
            var currentDate = new DateTimeOffset(2021, 5, 20, 12, 11, 10, TimeSpan.Zero);
            var travelDate  = new DateTimeOffset(2021, 6, 1, 10, 0, 0, TimeSpan.Zero);

            var expected = 160m;

            //act
            var result = price.CalculateLastMinuteDiscount(travelDate, currentDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_last_minute() {
            //arrange
            var price       = 123m;
            var currentDate = new DateTimeOffset(2021, 3, 4, 12, 11, 10, TimeSpan.Zero);
            var travelDate  = new DateTimeOffset(2022, 12, 1, 10, 0, 0, TimeSpan.Zero);

            var expected = 123m;

            //act
            var result = price.CalculateLastMinuteDiscount(travelDate, currentDate);

            //assert
            result.Should().Be(expected);
        }
    }
}