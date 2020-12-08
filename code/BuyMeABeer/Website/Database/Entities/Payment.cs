using System;

namespace Website.Database.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BeerId { get; set; }
        public string StripeSessionId { get; set; }
    }
}
