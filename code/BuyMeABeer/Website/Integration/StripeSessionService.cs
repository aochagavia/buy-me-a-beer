using Domain.Integration;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Options;

namespace Website.Integration
{
    public class StripeSessionService : IStripeSessionService
    {
        private readonly IOptions<DeploymentOptions> _deploymentOptions;

        public StripeSessionService(IOptions<DeploymentOptions> deploymentOptions)
        {
            _deploymentOptions = deploymentOptions;
        }

        public async Task<string> CreateStripeSession(string itemDescription, int itemPrice)
        {
            var options = new SessionCreateOptions
            {
                // TODO: use Controller + Action instead of hardcoding the url
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
                        Name = itemDescription,
                        Amount = itemPrice,
                        Currency = "EUR",
                        Quantity = 1,
                    }
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Id;
        }
    }
}
