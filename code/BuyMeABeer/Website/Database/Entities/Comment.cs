using System;

namespace Website.Database.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }
        public bool Approved { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
