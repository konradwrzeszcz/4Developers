using System;
using FluentAssertions;
using TravelAgency.DeclarativeCode;
using TravelAgency.DeclarativeCode.Domain;
using Xunit;

namespace Tests.DeclarativeCode {
    public class LoyaltyDiscountTests {
        [Fact]
        public void should_discount_by_loyalty() {
            //arrange
            var price       = 300m;
            var currentDate = new DateTimeOffset(2022, 2, 11, 12, 11, 10, TimeSpan.Zero);
            var userId      = "333";
            var travels = new[] {
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 6, 13, 12, 11, 10, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 8, 21, 3, 3, 0, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 12, 31, 23, 59, 59, TimeSpan.Zero),
                    BoughtBy = userId
                }
            };

            var expected = 240m;

            //act
            var result = price.CalculateLoyaltyDiscount(userId, travels, currentDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_by_loyalty_of_other_user() {
            //arrange
            var price       = 300m;
            var currentDate = new DateTimeOffset(2022, 2, 11, 12, 11, 10, TimeSpan.Zero);
            var userId      = "333";

            var otherUserId = "555";
            var otherUserTravels = new[] {
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 3, 12, 0, 0, 0, TimeSpan.Zero),
                    BoughtBy = otherUserId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 6, 13, 12, 11, 10, TimeSpan.Zero),
                    BoughtBy = otherUserId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 11, 23, 23, 59, 59, TimeSpan.Zero),
                    BoughtBy = otherUserId
                }
            };

            var expected = 300m;

            //act
            var result = price.CalculateLoyaltyDiscount(userId, otherUserTravels, currentDate);

            //assert
            result.Should().Be(expected);
        }

        [Fact]
        public void should_not_discount_not_loyal_user_by_loyalty() {
            //arrange
            var price       = 300m;
            var currentDate = new DateTimeOffset(2022, 2, 11, 12, 11, 10, TimeSpan.Zero);
            var userId      = "333";

            var travels = new[] {
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2019, 7, 7, 23, 59, 59, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 3, 12, 0, 0, 0, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2021, 6, 13, 12, 11, 10, TimeSpan.Zero),
                    BoughtBy = userId
                },
                new TravelDataStore.Travel {
                    From     = new DateTimeOffset(2022, 1, 14, 23, 59, 59, TimeSpan.Zero),
                    BoughtBy = userId
                }
            };

            var expected = 300m;

            //act
            var result = price.CalculateLoyaltyDiscount(userId, travels, currentDate);

            //assert
            result.Should().Be(expected);
        }
    }
}