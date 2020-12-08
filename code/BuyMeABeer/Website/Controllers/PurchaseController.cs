using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Website.Config;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly BeerOrderService _beerOrderService;
        private readonly IOptions<StripeOptions> _stripeOptions;

        public PurchaseController(BeerOrderService beerOrderService, IOptions<StripeOptions> stripeOptions)
        {
            _beerOrderService = beerOrderService;
            _stripeOptions = stripeOptions;
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

        public IActionResult PaymentSuccess(string sessionId)
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult PostComment()
        {
            if (!ModelState.IsValid)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            return View();
        }

        public IActionResult PaymentCancelled()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            var formModel = model.PurchaseForm;
            var beerProduct = _beerOrderService.GetBeerProduct(formModel.Product.Id);
            var payment = await _beerOrderService.PlaceOrder(formModel.Product.Id, formModel.Price);

            return View(new CheckoutModel
            {
                ProductDescription = beerProduct.Description,
                ProductPrice = beerProduct.Price ?? formModel.Price ?? 0,
                StripeSessionId = payment.StripeSessionId,
                StripePublicKey = _stripeOptions.Value.PublicKey,
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
