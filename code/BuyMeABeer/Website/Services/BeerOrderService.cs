using System;
using System.Threading.Tasks;
using Website.Database.Entities;
using Website.Models;

namespace Website.Services
{
    public class BeerOrderService
    {
        private readonly PaymentService _paymentService;

        public BeerOrderService(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<Payment> PlaceOrder(BeerProduct beerProduct, int? price)
        {
            if (beerProduct == null)
            {
                // TODO: maybe make this user-friendly
                throw new Exception("Beer product not found");
            }

            if (beerProduct.Price != null && price != null && beerProduct.Price != price)
            {
                // TODO: maybe make this user-friendly
                throw new Exception("Form price doesn't match beer price");
            }

            var payment = await _paymentService.CreatePayment(beerProduct, price);
            
            return payment;
        }
    }
}
