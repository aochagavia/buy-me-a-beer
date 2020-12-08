namespace Website.Models
{
    public class CheckoutModel
    {
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string StripeSessionId { get; set; }
        public string StripePublicKey { get; set; }
    }
}
