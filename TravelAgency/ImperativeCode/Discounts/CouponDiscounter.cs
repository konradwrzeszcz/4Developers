using System;

namespace TravelAgency.ImperativeCode.Discounts {
    public interface ICouponDiscounter {
        decimal Discount(decimal price, string couponCode);
    }

    public class CouponDiscounter : ICouponDiscounter {
        private readonly IDateTimeProvider _dateTimeProvider;
        public CouponDiscounter(IDateTimeProvider dateTimeProvider) => _dateTimeProvider = dateTimeProvider;

        public decimal Discount(decimal price, string couponCode) {
            var code2021             = "CHEAPER_TRAVEL_2021";
            var couponExpirationDate = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero);

            return couponCode == code2021 && _dateTimeProvider.GetUtcNow() < couponExpirationDate
                ? price * 0.8m
                : price;
        }
    }
}