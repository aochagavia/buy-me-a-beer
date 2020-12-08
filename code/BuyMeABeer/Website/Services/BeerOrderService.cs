using System;
using System.Linq;
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

        public BeerProduct[] AvailableBeerProducts()
        {
            return new[]
            {
                new BeerProduct
                {
                    Id = Guid.Parse("daefe6c3-416c-4696-9bad-e01a7313857e"),
                    Price = 2,
                    Description = "Small beer",
                },
                new BeerProduct
                {
                    Id = Guid.Parse("faf10ef7-a0ac-492b-958a-b0ba4a296062"),
                    Price = 5,
                    Description = "Medium beer",
                },
                new BeerProduct
                {
                    Id = Guid.Parse("cb77fc6a-c28a-4ee7-9881-3c0b0b78b8fa"),
                    Price = null,
                    Description = "Custom beer",
                },
            };
        }

        public BeerProduct GetBeerProduct(Guid beerProductId)
        {
            return AvailableBeerProducts().FirstOrDefault(prod => prod.Id == beerProductId);
        }
    }
}
