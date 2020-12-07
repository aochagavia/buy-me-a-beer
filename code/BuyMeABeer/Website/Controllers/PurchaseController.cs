using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Website.Database;
using Website.Database.Entities;
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
            // TODO: allow the user to post the price if the beer product doesn't have a price
            var beerProduct = _beerOrderService.GetBeerProduct(id);
            return View(beerProduct);
        }

        // TODO: the controller will return an empty http response if a GET is issued... Can we do something more user-friendly
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            // TODO: use real data from POST
            await _beerOrderService.PlaceOrder(Guid.Empty, "DummyNickname", "The answer is 42", null);
            return View();
        }
    }
}
