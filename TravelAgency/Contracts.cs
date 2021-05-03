using System;

namespace TravelAgency {
    public static class Contracts {
        public class GetTravelRequest {
            public string UserId             { get; set; }
            public string DiscountCouponCode { get; set; }

            public class Response {
                public Travel  Travel          { get; set; }
                public decimal DiscountedPrice { get; set; }
            }
        }

        public class BuyTravelRequest {
            public string UserId { get; set; }

            public class Response {
                public Travel Travel { get; set; }
            }
        }

        public class Travel {
            public string         Id          { get; set; }
            public string         Destination { get; set; }
            public DateTimeOffset From        { get; set; }
            public DateTimeOffset To          { get; set; }
            public decimal        Price       { get; set; }
        }
    }
}