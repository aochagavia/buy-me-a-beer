namespace Website.Models
{
    public class PurchaseFormModel
    {
        public BeerProduct Product { get; set; }
        public string Nickname { get; set; }
        public string Message { get; set; }
        public int? Price { get; set; }
    }
}
