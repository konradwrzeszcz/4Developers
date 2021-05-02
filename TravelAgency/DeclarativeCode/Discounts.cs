using System;
using System.Linq;
using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace TravelAgency.DeclarativeCode {
    public delegate decimal CalculateDiscount(string userId, string travelId, string couponCode);

    public static class Discounts {
        public const string Code2021 = "CHEAPER_TRAVEL_2021";

        public static decimal Calculate(
            string               userId,
            string               travelId,
            string               couponCode,
            Func<string, Travel> getTravel,
            Func<Travel[]>       listTravels,
            GetUtcNow            getUtcNow
        ) {
            var now     = getUtcNow();
            var travel  = getTravel(travelId);
            var travels = listTravels();

            return travel.Price
                .CalculateCouponDiscount(couponCode, now)
                .CalculateLastMinuteDiscount(travel.From, now)
                .CalculateLoyaltyDiscount(userId, travels, now);
        }

        public static decimal CalculateCouponDiscount(this decimal price, string couponCode, DateTimeOffset now) =>
            couponCode == Code2021 && now < new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero)
                ? price * 0.8m
                : price;

        public static decimal CalculateLastMinuteDiscount(this decimal price, DateTimeOffset travelStartDate,
            DateTimeOffset now) => travelStartDate.AddMonths(1) > now ? price * 0.8m : price;

        public static decimal CalculateLoyaltyDiscount(
            this decimal   price,
            string         userId,
            Travel[]       travels,
            DateTimeOffset now
        ) {
            const int minimumTravelCount = 3;
            var       lastYearStart      = new DateTimeOffset(now.Year - 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var       lastYearEnd        = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1);

            var userLastYearTravels = travels
                .Where(travel => travel.BoughtBy == userId && travel.From > lastYearStart && travel.From < lastYearEnd)
                .Count();

            return userLastYearTravels > minimumTravelCount
                ? price * 0.8m
                : price;
        }
    }
}