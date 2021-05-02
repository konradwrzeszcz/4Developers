using System;

namespace TravelAgency.ImperativeCode.Discounts {
    public interface ICouponDiscounter {
        decimal Discount(decimal price, string couponCode);
    }

    public class CouponDiscounter : ICouponDiscounter {
        public const string Code2021 = "CHEAPER_TRAVEL_2021";

        private readonly IDateTimeProvider _dateTimeProvider;

        public CouponDiscounter(IDateTimeProvider dateTimeProvider) => _dateTimeProvider = dateTimeProvider;

        public decimal Discount(decimal price, string couponCode) {
            if (couponCode == Code2021 &&
                _dateTimeProvider.GetUtcNow() < new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero))
                return price * 0.8m;

            return price;
        }
    }
}