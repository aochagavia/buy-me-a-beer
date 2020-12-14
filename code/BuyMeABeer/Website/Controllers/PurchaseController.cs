using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Website.Models;
using Website.Models.Form;
using Website.Options;
using Website.Services;

namespace Website.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly BeerOrderService _beerOrderService;
        private readonly CommentCreationService _commentCreationService;
        private readonly PaymentRepository _paymentRepository;
        private readonly BeerProductRepository _beerProductRepository;
        private readonly IOptions<StripeOptions> _stripeOptions;

        public PurchaseController(
            BeerOrderService beerOrderService,
            CommentCreationService commentCreationService,
            PaymentRepository paymentRepository,
            IOptions<StripeOptions> stripeOptions,
            BeerProductRepository beerProductRepository)
        {
            _beerOrderService = beerOrderService;
            _commentCreationService = commentCreationService;
            _paymentRepository = paymentRepository;
            _stripeOptions = stripeOptions;
            _beerProductRepository = beerProductRepository;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Checkout(HomeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            var beerProduct = _beerProductRepository.GetBeerProduct(model.BeerProductId);
            if (beerProduct == null)
            {
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            return View(new CheckoutFormModel
            {
                ProductId = beerProduct.Id,
                ProductDescription = beerProduct.Description,
                ProductPrice = beerProduct.Price,
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RedirectToStripe(CheckoutFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            var beerProduct = _beerProductRepository.GetBeerProduct(model.ProductId);
            var payment = await _beerOrderService.PlaceOrder(beerProduct, beerProduct.Price ?? model.ProductPrice);

            return View(new RedirectToStripeModel
            {
                StripeSessionId = payment.StripeSessionId,
                StripePublicKey = _stripeOptions.Value.PublicKey,
            });
        }

        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            var payment = await _paymentRepository.GetByStripeSessionId(sessionId);
            if (payment == null)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            return View(new CommentFormModel
            {
                AlreadyCommented = payment.Comment != null,
                PaymentId = payment.Id,
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(CommentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                // Invalid model should be prevented by the browser, so we don't need to be very user-friendly here
                return RedirectToAction(nameof(PurchaseController.Error));
            }

            var comment = await _commentCreationService.AddComment(model.PaymentId, model.Nickname, model.Message);

            return View(new CommentPostSuccessModel
            {
                Comment = comment,
            });
        }

        public IActionResult PaymentCancelled()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
