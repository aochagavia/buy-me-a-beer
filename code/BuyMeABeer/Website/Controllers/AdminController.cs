using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly PaymentRepository _paymentRepository;

        public AdminController(CommentRepository commentRepository, PaymentRepository paymentRepository)
        {
            _commentRepository = commentRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(new AdminHome
            {
                CommentCount = await _commentRepository.Count(),
                PaymentCount = await _paymentRepository.Count(),
            });
        }
    }
}