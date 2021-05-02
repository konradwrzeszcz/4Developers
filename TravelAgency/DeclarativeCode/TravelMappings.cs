namespace TravelAgency.DeclarativeCode {
    public static class TravelMappings {
        public static Contracts.Travel Map(this TravelDataStore.Travel source) =>
            new() {
                Id          = source.Id,
                Destination = source.Destination,
                From        = source.From,
                To          = source.To,
                Price       = source.Price
            };
    }
}