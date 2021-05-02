using System;

namespace TravelAgency.ImperativeCode {
    public interface IDateTimeProvider {
        DateTimeOffset GetUtcNow();
    }

    public class DateTimeProvider : IDateTimeProvider {
        public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
    }
}