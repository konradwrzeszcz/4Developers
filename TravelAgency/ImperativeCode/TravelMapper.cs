namespace TravelAgency.ImperativeCode {
    public interface ITravelMapper {
        Contracts.Travel Map(TravelProvider.Travel source);
    }

    public class TravelMapper : ITravelMapper {
        public Contracts.Travel Map(TravelProvider.Travel source) =>
            new() {
                Id          = source.Id,
                Destination = source.Destination,
                From        = source.From,
                To          = source.To,
                Price       = source.Price
            };
    }
}