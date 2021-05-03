using System;
using System.Collections.Generic;
using static TravelAgency.ImperativeCode.TravelProvider;

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
            const int minimumTravelCount = 3;

            var now           = _dateTimeProvider.GetUtcNow();
            var lastYearStart = new DateTimeOffset(now.Year - 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var lastYearEnd   = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1);

            var travels = _travelProvider.List();

            var userLastYearTravels                = new List<Travel>();
            var userBoughtMoreThan3TravelsLastYear = false;

            for (var i = 0; i < travels.Length; i++) {
                var travel = travels[i];
                if (travel.BoughtBy == userId && travel.From > lastYearStart && travel.From < lastYearEnd)
                    userLastYearTravels.Add(travel);

                if (userLastYearTravels.Count == minimumTravelCount) {
                    userBoughtMoreThan3TravelsLastYear = true;
                    break;
                }
            }

            return userBoughtMoreThan3TravelsLastYear
                ? price * 0.8m
                : price;
        }
    }
}