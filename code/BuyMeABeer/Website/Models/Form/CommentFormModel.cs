using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Form
{
    public class CommentFormModel
    {
        public Guid PaymentId { get; set; }
        public bool AlreadyCommented { get; set; }
        [StringLength(50), Required]
        public string Nickname { get; set; }
        [StringLength(280), Required]
        public string Message { get; set; }
    }
}
