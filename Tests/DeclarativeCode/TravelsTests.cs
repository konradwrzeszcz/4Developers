using System;
using FluentAssertions;
using TravelAgency.DeclarativeCode.Domain;
using Xunit;
using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace Tests.DeclarativeCode {
    public class TravelsTests {
        [Fact]
        public void should_buy_travel() {
            //arrange
            var userId = "345";
            var travel = new Travel {
                Id          = "123",
                Destination = "Paris",
                Price       = 1000m,
                From        = new DateTimeOffset(2020, 3, 1, 0, 0, 0, TimeSpan.Zero),
                To          = new DateTimeOffset(2020, 4, 1, 0, 0, 0, TimeSpan.Zero),
                Sold        = false,
                BoughtBy    = null
            };

            var expected = new Travel {
                Id          = "123",
                Destination = "Paris",
                Price       = 1000m,
                From        = new DateTimeOffset(2020, 3, 1, 0, 0, 0, TimeSpan.Zero),
                To          = new DateTimeOffset(2020, 4, 1, 0, 0, 0, TimeSpan.Zero),
                Sold        = true,
                BoughtBy    = userId
            };

            //act
            var result = travel.Buy(userId);

            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}