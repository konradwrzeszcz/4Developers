using System;
using FluentAssertions;
using TravelAgency;
using TravelAgency.ImperativeCode;
using Xunit;

namespace Tests.ImperativeCode {
    public class TravelMapperTests {
        [Fact]
        public void should_map() {
            //arrange
            var sut = new TravelMapper();

            var source = new TravelProvider.Travel {
                Id          = "random_id",
                Destination = "random_destination",
                From        = new DateTimeOffset(2021, 6, 1, 10, 0, 0, TimeSpan.Zero),
                To          = new DateTimeOffset(2021, 6, 7, 20, 0, 0, TimeSpan.Zero),
                Price       = 12345.67m,
                Sold        = true,
                BoughtBy    = "random_user_id"
            };

            var expected = new Contracts.Travel {
                Id          = "random_id",
                Destination = "random_destination",
                From        = new DateTimeOffset(2021, 6, 1, 10, 0, 0, TimeSpan.Zero),
                To          = new DateTimeOffset(2021, 6, 7, 20, 0, 0, TimeSpan.Zero),
                Price       = 12345.67m
            };

            //act
            var result = sut.Map(source);

            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}