using System;
using Microsoft.Extensions.DependencyInjection;
using static TravelAgency.DeclarativeCode.GetUtcNowDecorator;

namespace TravelAgency.DeclarativeCode {
    public static class ServiceConfigurator {
        public static IServiceCollection AddDeclarativeTravels(this IServiceCollection services) {
            services.AddSingleton<TravelDataStore>();

            GetUtcNow getUtcNow            = () => DateTimeOffset.UtcNow;
            GetUtcNow getUtcNowWithLogging = () => WithLogging(getUtcNow);
            services.AddSingleton<GetUtcNow>(getUtcNowWithLogging);

            return services;
        }
    }
}