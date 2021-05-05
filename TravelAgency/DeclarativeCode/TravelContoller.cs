using Microsoft.AspNetCore.Mvc;
using TravelAgency.DeclarativeCode.Domain;
using static TravelAgency.Contracts;
using static TravelAgency.DeclarativeCode.Domain.ApplyDiscountModule;
using static TravelAgency.DeclarativeCode.Domain.DiscountRuleModule;

namespace TravelAgency.DeclarativeCode
{
    [ApiController]
    [Route("declarative/travels")]
    public class TravelController : ControllerBase
    {
        private readonly TravelDataStore _travelDataStore;
        private readonly GetUtcNow _getUtcNow;

        public TravelController(TravelDataStore travelDataStore, GetUtcNow getUtcNow)
        {
            _travelDataStore = travelDataStore;
            _getUtcNow = getUtcNow;
        }

        [HttpGet("{travelId}")]
        public ActionResult<GetTravelRequest.Response> Get([FromQuery] GetTravelRequest request, string travelId)
        {
            var travel = _travelDataStore.Get(travelId);
            if (travel is null)
                return NotFound();

            var result = CalculateDiscount(new Amount(travel.Price))
                .When(now => Coupon.Of(request.DiscountCouponCode).Valid(now))
                .When(LastMinuteBooking(travel.From))
                .When(LoyalCustomer(request.UserId, _travelDataStore.List()))
                .Invoke(_getUtcNow());

            return new GetTravelRequest.Response
            {
                Travel = travel.Map(),
                DiscountedPrice = result.Value
            };
        }

        [HttpPost("{travelId}/buy")]
        public ActionResult<BuyTravelRequest.Response> Buy([FromQuery] BuyTravelRequest request, string travelId)
        {
            var travel = _travelDataStore.Get(travelId);
            if (travel is null)
                return NotFound();

            var boughtTravel = travel.Buy(request.UserId);

            _travelDataStore.Update(travel.Id, boughtTravel);

            return new BuyTravelRequest.Response
            {
                Travel = boughtTravel.Map()
            };
        }
    }
}