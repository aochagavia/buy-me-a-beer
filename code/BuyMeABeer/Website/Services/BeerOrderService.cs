using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Database;
using Website.Database.Entities;
using Website.Models;

namespace Website.Services
{
    public class BeerOrderService
    {
        private readonly WebsiteDbContext _db;
        private readonly PaymentService _paymentService;

        public BeerOrderService(WebsiteDbContext db, PaymentService paymentService)
        {
            _db = db;
            _paymentService = paymentService;
        }

        public async Task<Payment> PlaceOrder(Guid beerId, int? price) // string nickname, string message
        {
            var beerProduct = GetBeerProduct(beerId);
            if (beerProduct == null)
            {
                // TODO: maybe make this user-friendly
                throw new Exception("Beer product not found");
            }

            if (beerProduct.Price != null && price != null)
            {
                // TODO: maybe make this user-friendly
                throw new Exception("Beer product doesn't accept a custom price");
            }

            var payment = await _paymentService.CreatePayment(beerProduct, price);

            //if (!string.IsNullOrWhiteSpace(message))
            //{
            //    var comment = new Comment
            //    {
            //        Payment = payment,
            //        Nickname = string.IsNullOrWhiteSpace(nickname) ? null : nickname,
            //        Message = message,
            //        CreatedUtc = DateTimeOffset.UtcNow,
            //    };

            //    _db.Add(comment);
            //}
            
            await _db.SaveChangesAsync();
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
