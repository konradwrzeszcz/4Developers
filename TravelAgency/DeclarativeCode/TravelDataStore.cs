using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelAgency.DeclarativeCode {
    public class TravelDataStore {
        private List<Travel> _travelsCollection = new();

        public Travel[] List()               => _travelsCollection.ToArray();
        public Travel   Get(string travelId) => _travelsCollection.SingleOrDefault(travel => travel.Id == travelId);

        public Travel Update(string travelId, Action<Travel> update) {
            var travel = Get(travelId);

            if (travel is null) throw new ArgumentException("Travel doesn't exist");

            update(travel);

            return travel;
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