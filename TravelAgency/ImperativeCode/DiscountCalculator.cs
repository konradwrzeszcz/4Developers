using TravelAgency.ImperativeCode.Discounts;

namespace TravelAgency.ImperativeCode {
    public interface IDiscountCalculator {
        decimal Calculate(string userId, string travelId, string couponCode);
    }

    public class DiscountCalculator : IDiscountCalculator {
        private readonly ITravelProvider       _travelProvider;
        private readonly ICouponDiscounter     _couponDiscounter;
        private readonly ILastMinuteDiscounter _lastMinuteDiscounter;
        private readonly ILoyaltyDiscounter    _loyaltyDiscounter;

        public DiscountCalculator(
            ITravelProvider       travelProvider,
            ICouponDiscounter     couponDiscounter,
            ILastMinuteDiscounter lastMinuteDiscounter,
            ILoyaltyDiscounter    loyaltyDiscounter) {
            _travelProvider       = travelProvider;
            _couponDiscounter     = couponDiscounter;
            _lastMinuteDiscounter = lastMinuteDiscounter;
            _loyaltyDiscounter    = loyaltyDiscounter;
        }

        public decimal Calculate(string userId, string travelId, string couponCode) {
            var travel = _travelProvider.Get(travelId);

            decimal discountedPrice = travel.Price;

            discountedPrice = _couponDiscounter.Discount(discountedPrice, couponCode);
            discountedPrice = _lastMinuteDiscounter.Discount(discountedPrice, travel.From);
            discountedPrice = _loyaltyDiscounter.Discount(discountedPrice, userId);

            return discountedPrice;
        }
    }
}