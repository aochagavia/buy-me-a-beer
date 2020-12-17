using System;

namespace Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BeerId { get; set; }
        public string StripeSessionId { get; set; } = default!; // Assume this field will be populated properly by the repository
        public int Amount { get; set; }
        public Comment? Comment { get; set; }
    }
}
