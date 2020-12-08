using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly BeerOrderService _beerOrderService;

        public PurchaseController(BeerOrderService beerOrderService)
        {
            _beerOrderService = beerOrderService;
        }

        [Route("{id}")]
        public IActionResult Index(Guid id)
        {
            var beerProduct = _beerOrderService.GetBeerProduct(id);
            return View(new PurchaseFormModel
            {
                Product = beerProduct,
            });
        }

        // TODO: the controller will return an empty http response if a GET is issued... Can we do something more user-friendly?
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(PurchaseFormModel model)
        {
            await _beerOrderService.PlaceOrder(model.Product.Id, model.Nickname, model.Message, model.Price);
            return View();
        }
    }
}
