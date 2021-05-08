using System;
using FluentAssertions;
using TravelAgency;
using TravelAgency.DeclarativeCode;
using Xunit;

namespace Tests.DeclarativeCode {
    public class TravelMappingsTests {
        [Fact]
        public void should_map() {
            //arrange
            var source = new TravelDataStore.Travel
            {
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
            var result = source.Map();
            
            //assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}