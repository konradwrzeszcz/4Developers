using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.ImperativeCode.Discounts;
using static TravelAgency.Contracts;

namespace TravelAgency.ImperativeCode {
    [ApiController]
    [Route("imperative/travels")]
    public class TravelController : ControllerBase {
        private readonly ITravelProvider       _travelProvider;
        private readonly ICouponDiscounter     _couponDiscounter;
        private readonly ILoyaltyDiscounter    _loyaltyDiscounter;
        private readonly ILastMinuteDiscounter _lastMinuteDiscounter;
        private readonly ITravelMapper         _travelMapper;

        public TravelController(
            ITravelProvider       travelProvider,
            ICouponDiscounter     couponDiscounter,
            ILoyaltyDiscounter    loyaltyDiscounter,
            ILastMinuteDiscounter lastMinuteDiscounter,
            ITravelMapper         travelMapper) {
            _travelProvider       = travelProvider;
            _couponDiscounter     = couponDiscounter;
            _loyaltyDiscounter    = loyaltyDiscounter;
            _lastMinuteDiscounter = lastMinuteDiscounter;
            _travelMapper         = travelMapper;
        }

        [HttpGet("{travelId}")]
        public ActionResult<GetTravelRequest.Response> Get([FromQuery] GetTravelRequest request, string travelId) {
            var travel = _travelProvider.Get(travelId);
            if (travel is null)
                return NotFound();

            var discountedPrice = travel.Price;

            discountedPrice = _couponDiscounter.Discount(discountedPrice, request.DiscountCouponCode);
            discountedPrice = _lastMinuteDiscounter.Discount(discountedPrice, travel.From);
            discountedPrice = _loyaltyDiscounter.Discount(discountedPrice, request.UserId);

            return new GetTravelRequest.Response {
                Travel          = _travelMapper.Map(travel),
                DiscountedPrice = discountedPrice
            };
        }

        [HttpPost("{travelId}/buy")]
        public ActionResult<BuyTravelRequest.Response> Buy([FromQuery] BuyTravelRequest request, string travelId) {
            var travel = _travelProvider.Get(travelId);
            if (travel is null)
                return NotFound();

            travel.Sold     = true;
            travel.BoughtBy = request.UserId;

            var boughtTravel = _travelProvider.Update(travel.Id, travel);

            return new BuyTravelRequest.Response {
                Travel = _travelMapper.Map(boughtTravel)
            };
        }
    }
}