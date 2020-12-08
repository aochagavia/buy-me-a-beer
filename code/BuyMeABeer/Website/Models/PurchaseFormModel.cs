using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class PurchaseFormModel
    {
        public BeerProduct Product { get; set; }
        public int? Price { get; set; }
    }
}
