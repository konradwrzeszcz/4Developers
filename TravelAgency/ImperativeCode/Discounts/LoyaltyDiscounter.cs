using System;

namespace TravelAgency.ImperativeCode.Discounts {
    public interface ILoyaltyDiscounter {
        decimal Discount(decimal price, string userId);
    }

    public class LoyaltyDiscounter : ILoyaltyDiscounter {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITravelProvider   _travelProvider;

        public LoyaltyDiscounter(IDateTimeProvider dateTimeProvider, ITravelProvider travelProvider) {
            _dateTimeProvider = dateTimeProvider;
            _travelProvider   = travelProvider;
        }

        public decimal Discount(decimal price, string userId) {
            const int minimumNumberOfTravels = 3;

            var now           = _dateTimeProvider.GetUtcNow();
            var lastYearStart = new DateTimeOffset(now.Year - 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var lastYearEnd   = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1);

            var travels = _travelProvider.List();

            var userLastYearTravelsCount                 = 0;
            var userBoughtMinimumNumberOfTravelsLastYear = false;

            foreach (var travel in travels) {
                if (travel.BoughtBy == userId && travel.From >= lastYearStart && travel.From <= lastYearEnd)
                    userLastYearTravelsCount++;
                
                if (userLastYearTravelsCount > minimumNumberOfTravels) {
                    userBoughtMinimumNumberOfTravelsLastYear = true;
                    break;
                }
            }

            return userBoughtMinimumNumberOfTravelsLastYear
                ? price * 0.8m
                : price;
        }
    }
}