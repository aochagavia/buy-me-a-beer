using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Database.Entities;
using Website.Models;

namespace Website.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;

        public PaymentService(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> CreatePayment(BeerProduct beerProduct, int? customPrice)
        {
            //IUrlHelper helper;
            //var successUrl = helper.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext
            //{

            //});

            var price = (beerProduct.Price ?? customPrice).Value * 100;

            var options = new SessionCreateOptions
            {
                SuccessUrl = "http://localhost:3000/Purchase/PaymentSuccess?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:3000/Purchase/PaymentCancelled",
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
