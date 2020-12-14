using System;
using System.Linq;
using Website.Models;

namespace Website.Services
{
    public class BeerProductRepository
    {
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
