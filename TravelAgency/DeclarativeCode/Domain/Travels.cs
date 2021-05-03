using static TravelAgency.DeclarativeCode.TravelDataStore;

namespace TravelAgency.DeclarativeCode.Domain {
    public static class Travels {
        public static Travel Buy(this Travel travelToBuy, string userId) {
            travelToBuy.Sold     = true;
            travelToBuy.BoughtBy = userId;

            return travelToBuy;
        }
    }
}