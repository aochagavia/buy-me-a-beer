using System.Threading.Tasks;
using Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPaymentRepository _paymentRepository;

        public AdminController(ICommentRepository commentRepository, IPaymentRepository paymentRepository)
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