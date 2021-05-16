using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace TravelAgency.DeclarativeCode.Domain {
    public static class Travels {
        public static Travel Buy(this Travel travelToBuy, string userId) => new() {
            Id          = travelToBuy.Id,
            Destination = travelToBuy.Destination,
            Price       = travelToBuy.Price,
            From        = travelToBuy.From,
            To          = travelToBuy.To,
            Sold        = true,
            BoughtBy    = userId
        };
    }
}