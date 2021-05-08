using Microsoft.Extensions.DependencyInjection;
using TravelAgency.ImperativeCode.Discounts;

namespace TravelAgency.ImperativeCode {
    public static class ServiceConfigurator {
        public static IServiceCollection AddImperativeTravels(this IServiceCollection services) {
            services.AddSingleton<ITravelProvider, TravelProvider>();
            services.AddSingleton<ITravelMapper, TravelMapper>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ICouponDiscounter, CouponDiscounter>();
            services.AddSingleton<ILastMinuteDiscounter, LastMinuteDiscounter>();
            services.AddSingleton<ILoyaltyDiscounter, LoyaltyDiscounter>();

            return services;
        }
    }
}