using Domain.Entities;
using System;

namespace Website.Models.Form
{
    public class HomeFormModel
    {
        public Comment[] Comments { get; set; }
        public BeerProduct[] BeerProducts { get; set; }
        public Guid BeerProductId { get; set; }
    }
}
