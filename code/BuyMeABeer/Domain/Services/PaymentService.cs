using Domain.Entities;
using Domain.Integration;
using Domain.Repositories;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStripeSessionService _stripeSessionService;

        public PaymentService(IPaymentRepository paymentRepository, IStripeSessionService stripeSessionService)
        {
            _paymentRepository = paymentRepository;
            _stripeSessionService = stripeSessionService;
        }

        public async Task<Payment> CreatePayment(BeerProduct beerProduct, int? customPrice)
        {
            var price = beerProduct.Price ?? customPrice;
            if (price == null)
            {
                // TODO: use custom exception
                throw new System.Exception("This code should be unrechable");
            }
            var priceTimes100 = price.Value * 100;
            var sessionId = await _stripeSessionService.CreateStripeSession(beerProduct.Description, priceTimes100);
            return await _paymentRepository.Create(beerProduct.Id, sessionId, priceTimes100);
        }
    }
}
