using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Form
{
    public class CheckoutFormModel
    {
        public Guid ProductId { get; set; }
        public string ProductDescription { get; set; }
        [Required]
        public int? ProductPrice { get; set; }
    }
}
