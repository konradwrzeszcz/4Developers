using Microsoft.AspNetCore.Mvc;
using TravelAgency.DeclarativeCode.Domain;
using static TravelAgency.Contracts;

namespace TravelAgency.DeclarativeCode {
    [ApiController]
    [Route("declarative/travels")]
    public class TravelController : ControllerBase {
        private readonly TravelDataStore _travelDataStore;
        private readonly GetUtcNow       _getUtcNow;

        public TravelController(TravelDataStore travelDataStore, GetUtcNow getUtcNow) {
            _travelDataStore = travelDataStore;
            _getUtcNow       = getUtcNow;
        }

        [HttpGet("{travelId}")]
        public ActionResult<GetTravelRequest.Response> Get([FromQuery] GetTravelRequest request, string travelId) {
            var travel = _travelDataStore.Get(travelId);
            if (travel is null)
                return NotFound();

            var now = _getUtcNow();

            var discountedPrice = travel.Price
                .CalculateCouponDiscount(request.DiscountCouponCode, _getUtcNow)
                .CalculateLastMinuteDiscount(travel.From, now)
                .CalculateLoyaltyDiscount(request.UserId, _travelDataStore.List(), now);

            return new GetTravelRequest.Response {
                Travel          = travel.Map(),
                DiscountedPrice = discountedPrice
            };
        }

        [HttpPost("{travelId}/buy")]
        public ActionResult<BuyTravelRequest.Response> Buy([FromQuery] BuyTravelRequest request, string travelId) {
            var travel = _travelDataStore.Get(travelId);
            if (travel is null)
                return NotFound();

            var boughtTravel = travel.Buy(request.UserId);

            _travelDataStore.Update(travel.Id, boughtTravel);

            return new BuyTravelRequest.Response {
                Travel = boughtTravel.Map()
            };
        }
    }
}