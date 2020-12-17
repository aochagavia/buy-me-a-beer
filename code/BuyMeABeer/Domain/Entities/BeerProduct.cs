using System;

namespace Domain.Entities
{
    public class BeerProduct
    {
        public Guid Id { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; } = default!; // Assume this field will be populated properly by the repository
    }
}
