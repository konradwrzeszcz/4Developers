using System;

namespace TravelAgency.ImperativeCode.Discounts {
    public interface ILastMinuteDiscounter {
        decimal Discount(decimal price, DateTimeOffset travelStartDate);
    }

    public class LastMinuteDiscounter : ILastMinuteDiscounter {
        private readonly IDateTimeProvider _dateTimeProvider;

        public LastMinuteDiscounter(IDateTimeProvider dateTimeProvider) => _dateTimeProvider = dateTimeProvider;

        public decimal Discount(decimal price, DateTimeOffset travelStartDate)
            => travelStartDate.AddMonths(-1) < _dateTimeProvider.GetUtcNow()
                ? price * 0.8m
                : price;
    }
}