using System;
using System.Linq;
using static TravelAgency.CrazyFunctional.Domain.DiscountRuleModule;
using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace TravelAgency.CrazyFunctional.Domain {
    public record CouponCode(string Value);

    public record Amount(decimal Value);

    public record Percentage(decimal Value);

    public record Coupon(CouponCode Code, DateTimeOffset ExpirationDate) {
        public bool Valid(DateTimeOffset now)
            => Code == new CouponCode("CHEAPER_TRAVEL_2021") && now < ExpirationDate;

        public static Coupon Of(string code)
            => new(new CouponCode(code), new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    public record Discount(Percentage Percentage) {
        public Amount Apply(Amount price)
            => new(price.Value * Percentage.Value);
    }

    public static class ApplyDiscountModule {
        public delegate Amount ApplyDiscount(DateTimeOffset now);

        public static ApplyDiscount CalculateDiscount(Amount price)
            => _ => price;

        public static ApplyDiscount When(this ApplyDiscount price, DiscountRule discountRule)
            => When(price, new Discount(new Percentage(0.8m)), discountRule);

        static ApplyDiscount When(this ApplyDiscount price, Discount discount, DiscountRule discountRule)
            => now => discountRule(now) ? discount.Apply(price(now)) : price(now);
    }


    public static class DiscountRuleModule {
        public delegate bool DiscountRule(DateTimeOffset now);

        public static DiscountRule LastMinuteBooking(DateTimeOffset travelStartDate)
            => now => travelStartDate.AddMonths(-1) < now;

        public static DiscountRule LoyalCustomer(string userId, Travel[] travels)
            => now => {
                const int minimumTravelCount = 3;

                var lastYearStart = new DateTimeOffset(now.Year - 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
                var lastYearEnd   = new DateTimeOffset(now.Year, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(-1);

                var userLastYearTravels = travels
                    .Count(travel =>
                        travel.BoughtBy == userId && travel.From >= lastYearStart && travel.From <= lastYearEnd);

                return userLastYearTravels >= minimumTravelCount;
            };
    }
}