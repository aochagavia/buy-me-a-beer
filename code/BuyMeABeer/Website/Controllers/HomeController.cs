using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Models.Form;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBeerProductRepository _beerProductRepository;

        public HomeController(ICommentRepository commentRepository, IBeerProductRepository beerProductRepository)
        {
            _commentRepository = commentRepository;
            _beerProductRepository = beerProductRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(new HomeFormModel
            {
                BeerProducts = _beerProductRepository.AvailableBeerProducts(),
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