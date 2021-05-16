using System;
using Serilog;

namespace TravelAgency.DeclarativeCode {
    public delegate DateTimeOffset GetUtcNow();

    public static class GetUtcNowDecorator {
        public static DateTimeOffset WithLogging(GetUtcNow getUtcNow) {
            var now = getUtcNow();
            Log.Information("Current time {CurrentTime}", now);

            return now;
        }
    }
}