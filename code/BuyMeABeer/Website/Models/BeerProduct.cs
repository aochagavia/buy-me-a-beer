using System;

namespace Website.Models
{
    public class BeerProduct
    {
        public Guid Id { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
    }
}
