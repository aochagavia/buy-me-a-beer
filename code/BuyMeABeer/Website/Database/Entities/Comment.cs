using System;

namespace Website.Database.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public string Nickname { get; set; } // TODO: make sure the length is within certain limits
        public string Message { get; set; } // TODO: make sure the length is within certain limits
        public bool Approved { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
