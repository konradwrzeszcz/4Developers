using System;
using System.Linq;
using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace TravelAgency.DeclarativeCode.Domain {
    public static class Discounts {
        public static decimal CalculateCouponDiscount(this decimal price, string couponCode, DateTimeOffset now) {
            var code2021             = "CHEAPER_TRAVEL_2021";
            var couponExpirationDate = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero);

            return couponCode == code2021 && now < couponExpirationDate
                ? price * 0.8m
                : price;
        }

        public static decimal CalculateLastMinuteDiscount(
            this decimal price, DateTimeOffset travelStartDate, DateTimeOffset now
        ) => travelStartDate.AddMonths(-1) < now ? price * 0.8m : price;

        public static decimal CalculateLoyaltyDiscount(
            this decimal price, string userId, Travel[] travels, DateTimeOffset now
        ) {
            const int minimumNumberOfTravels = 3;

            var lastYearStart = new DateTimeOffset(now.Year - 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var lastYearEnd   = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1);

            var userBoughtMinimumNumberOfTravelsLastYear = travels
                .Where(travel =>
                    travel.BoughtBy == userId && travel.From >= lastYearStart && travel.From <= lastYearEnd
                )
                .Skip(minimumNumberOfTravels)
                .Any();

            return userBoughtMinimumNumberOfTravelsLastYear
                ? price * 0.8m
                : price;
        }
    }
}