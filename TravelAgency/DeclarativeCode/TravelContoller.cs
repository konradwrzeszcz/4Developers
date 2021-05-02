using Microsoft.AspNetCore.Mvc;
using static TravelAgency.Contracts;

namespace TravelAgency.DeclarativeCode {
    [ApiController]
    [Route("declarative/travels")]
    public class TravelController : ControllerBase {
        private readonly TravelDataStore   _travelDataStore;
        private readonly CalculateDiscount _calculateDiscount;

        public TravelController(TravelDataStore travelDataStore, CalculateDiscount calculateDiscount) {
            _travelDataStore   = travelDataStore;
            _calculateDiscount = calculateDiscount;
        }

        [HttpGet("{id}")]
        public GetTravelRequest.Response Get([FromQuery] GetTravelRequest request, string id) {
            var travel = _travelDataStore.Get(request.TravelId);
            if (travel is null)
                NotFound();

            var discountedPrice = _calculateDiscount(request.UserId, travel!.Id, request.DiscountCouponCode);

            return new GetTravelRequest.Response {
                Travel          = travel.Map(),
                DiscountedPrice = discountedPrice
            };
        }

        [HttpPost("{id}/buy")]
        public BuyTravelRequest.Response Buy([FromQuery] BuyTravelRequest request, string id) {
            var travel = _travelDataStore.Get(request.TravelId);
            if (travel is null)
                NotFound();

            var boughtTravel = _travelDataStore.Update(travel!.Id, t => {
                t.Sold     = true;
                t.BoughtBy = request.UserId;
            });

            return new BuyTravelRequest.Response {
                Travel = boughtTravel.Map()
            };
        }
    }
}