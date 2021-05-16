using System;
using Microsoft.Extensions.DependencyInjection;
using static TravelAgency.DeclarativeCode.GetUtcNowDecorator;

namespace TravelAgency.DeclarativeCode {
    public static class ServiceConfigurator {
        public static IServiceCollection AddDeclarativeTravels(this IServiceCollection services) {
            services.AddSingleton<TravelDataStore>();
            services.AddSingleton<GetUtcNow>(() => DateTimeOffset.UtcNow);

            return services;
        }
    }
}