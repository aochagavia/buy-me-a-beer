using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class PurchaseFormModel
    {
        public BeerProduct Product { get; set; }
        [StringLength(50)]
        public string Nickname { get; set; }
        [StringLength(280)]
        public string Message { get; set; }
        public int? Price { get; set; }
    }
}
