using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Database;
using Website.Database.Entities;
using Website.Models;

namespace Website.Services
{
    public class PaymentService
    {
        private readonly WebsiteDbContext _db;

        public PaymentService(WebsiteDbContext db)
        {
            _db = db;
        }

        public async Task<Payment> CreatePayment(BeerProduct beerProduct, int? customPrice)
        {
            //IUrlHelper helper;
            //var successUrl = helper.Action(new Microsoft.AspNetCore.Mvc.Routing.UrlActionContext
            //{

            //});
            
            // Check that the prices are available, or create them...

            var options = new SessionCreateOptions
            {
                SuccessUrl = "http://localhost:3000/Purchase/PaymentSuccess/{CHECKOUT_SESSION_ID}",
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
                        Amount = (beerProduct.Price ?? customPrice) * 100, // TODO: use a real id
                        Currency = "EUR",
                        Quantity = 1,
                    }
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            var payment = new Payment
            {
                BeerId = beerProduct.Id,
                StripeSessionId = session.Id,
            };
            _db.Add(payment);

            return payment;
        }
    }
}
