using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelAgency.ImperativeCode {
    public interface ITravelProvider {
        TravelProvider.Travel   Get(string travelId);
        TravelProvider.Travel[] List();
        TravelProvider.Travel   Update(string travelId, TravelProvider.Travel newValue);
    }

    public class TravelProvider : ITravelProvider {
        private Dictionary<string, Travel> _travelsCollection = new() {
            {
                "1", new Travel {
                    Id          = "1",
                    Destination = "Toronto",
                    From        = new DateTimeOffset(2021, 6, 1, 10, 0, 0, TimeSpan.Zero),
                    To          = new DateTimeOffset(2021, 6, 7, 20, 0, 0, TimeSpan.Zero),
                    Price       = 3000m,
                    Sold        = false,
                    BoughtBy    = null
                }
            }, {
                "2", new Travel {
                    Id          = "2",
                    Destination = "New York",
                    From        = new DateTimeOffset(2021, 7, 10, 10, 0, 0, TimeSpan.Zero),
                    To          = new DateTimeOffset(2021, 7, 12, 20, 0, 0, TimeSpan.Zero),
                    Price       = 500m,
                    Sold        = true,
                    BoughtBy    = "555"
                }
            }, {
                "3", new Travel {
                    Id          = "3",
                    Destination = "Radomsko",
                    From        = new DateTimeOffset(2021, 8, 1, 10, 0, 0, TimeSpan.Zero),
                    To          = new DateTimeOffset(2021, 8, 31, 20, 0, 0, TimeSpan.Zero),
                    Price       = 100m,
                    Sold        = false,
                    BoughtBy    = null
                }
            }
        };

        public Travel[] List() => _travelsCollection.Select(t => t.Value).ToArray();

        public Travel Get(string travelId) =>
            _travelsCollection.Select(t => t.Value).SingleOrDefault(travel => travel.Id == travelId);

        public Travel Update(string travelId, Travel newValue) {
            var travel = Get(travelId);

            if (travel is null) throw new ArgumentException("Travel doesn't exist");

            _travelsCollection[travelId] = newValue;

            return newValue;
        }

        public class Travel {
            public string         Id          { get; set; }
            public string         Destination { get; set; }
            public DateTimeOffset From        { get; set; }
            public DateTimeOffset To          { get; set; }
            public decimal        Price       { get; set; }
            public bool           Sold        { get; set; }
            public string         BoughtBy    { get; set; }
        }
    }
}