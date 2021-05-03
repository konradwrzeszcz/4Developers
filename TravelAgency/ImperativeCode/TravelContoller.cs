using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using static TravelAgency.Contracts;

namespace TravelAgency.ImperativeCode {
    [ApiController]
    [Route("imperative/travels")]
    public class TravelController : ControllerBase {
        private readonly ITravelProvider     _travelProvider;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly ITravelMapper       _travelMapper;

        public TravelController(
            ITravelProvider     travelProvider,
            IDiscountCalculator discountCalculator,
            ITravelMapper       travelMapper) {
            _travelProvider     = travelProvider;
            _discountCalculator = discountCalculator;
            _travelMapper       = travelMapper;
        }

        [HttpGet("{id}")]
        public ActionResult<GetTravelRequest.Response> Get([FromQuery] GetTravelRequest request, string id) {
            var travel = _travelProvider.Get(request.TravelId);
            if (travel is null)
                return NotFound();

            var discountedPrice = _discountCalculator.Calculate(request.UserId, travel.Id, request.DiscountCouponCode);

            return new GetTravelRequest.Response {
                Travel          = _travelMapper.Map(travel),
                DiscountedPrice = discountedPrice
            };
        }

        [HttpPost("{id}/buy")]
        public ActionResult<BuyTravelRequest.Response> Buy([FromQuery] BuyTravelRequest request, string id) {
            var travel = _travelProvider.Get(request.TravelId);
            if (travel is null)
                return NotFound();

            var boughtTravel = _travelProvider.Update(travel.Id, t => {
                t.Sold     = true;
                t.BoughtBy = request.UserId;
            });

            return new BuyTravelRequest.Response {
                Travel = _travelMapper.Map(boughtTravel)
            };
        }
    }
}