using Domain.Entities;
using System;

namespace Domain.Repositories
{
    public interface IBeerProductRepository
    {
        BeerProduct[] AvailableBeerProducts();
        BeerProduct GetBeerProduct(Guid beerProductId);
    }
}