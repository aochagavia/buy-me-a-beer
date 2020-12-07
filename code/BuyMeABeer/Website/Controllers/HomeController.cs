using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly BeerOrderService _beerOrderService;

        public HomeController(CommentRepository commentRepository, BeerOrderService beerOrderService)
        {
            _commentRepository = commentRepository;
            _beerOrderService = beerOrderService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new IndexViewModel
            {
                BeerProducts = _beerOrderService.AvailableBeerProducts(),
                Comments = await _commentRepository.LatestComments(),
            });
        }

        public IActionResult WhatIsThis()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}