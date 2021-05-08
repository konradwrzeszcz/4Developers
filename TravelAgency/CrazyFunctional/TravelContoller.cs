using Microsoft.AspNetCore.Mvc;
using TravelAgency.CrazyFunctional.Domain;
using TravelAgency.DeclarativeCode;
using static TravelAgency.Contracts;
using static TravelAgency.CrazyFunctional.Domain.ApplyDiscountModule;
using static TravelAgency.CrazyFunctional.Domain.DiscountRuleModule;

namespace TravelAgency.CrazyFunctional
{
    [ApiController]
    [Route("crazy-functional/travels")]
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
                .When(Coupon.Of(request.DiscountCouponCode).Valid)
                .When(LastMinuteBooking(travel.From))
                .When(LoyalCustomer(request.UserId, _travelDataStore.List()))
                .Invoke(_getUtcNow());

            return new GetTravelRequest.Response
            {
                Travel = travel.Map(),
                DiscountedPrice = result.Value
            };
        }
    }
}