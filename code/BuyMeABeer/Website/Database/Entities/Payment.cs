using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Database.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid BeerId { get; set; }
        [Required]
        public string StripeSessionId { get; set; }
        public int Amount { get; set; }
        public Comment Comment { get; set; }
    }
}
