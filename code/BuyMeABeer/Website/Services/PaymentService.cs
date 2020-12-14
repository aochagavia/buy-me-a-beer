using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Database.Entities;
using Website.Models;
using Website.Options;

namespace Website.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IOptions<DeploymentOptions> _deploymentOptions;

        public PaymentService(PaymentRepository paymentRepository, IOptions<DeploymentOptions> deploymentOptions)
        {
            _paymentRepository = paymentRepository;
            _deploymentOptions = deploymentOptions;
        }

        public async Task<Payment> CreatePayment(BeerProduct beerProduct, int? customPrice)
        {
            // TODO: use Controller + Action instead of hardcoding the url

            var price = (beerProduct.Price ?? customPrice).Value * 100;

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{_deploymentOptions.Value.BaseUrl}/Purchase/PaymentSuccess?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_deploymentOptions.Value.BaseUrl}/Purchase/PaymentCancelled",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                    "ideal",
                    "bancontact",
                    // TODO: what other payment methods should we accept? What happens if we leave the list empty?
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = beerProduct.Description,
                        Amount = price,
                        Currency = "EUR",
                        Quantity = 1,
                    }
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return await _paymentRepository.Create(beerProduct.Id, session.Id, price);
        }
    }
}
