using System;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = default!; // Assume this field will be populated properly by the repository
        public string? Nickname { get; set; }
        public string? Message { get; set; }
        public bool Approved { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
