using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Website.Database;
using Website.Models;

namespace Website.Controllers
{
    // [Authorize] // TODO: remove this Authorize, since this was just for testing
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebsiteDbContext _db;

        public HomeController(ILogger<HomeController> logger, WebsiteDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: pass down the available beer offerings and their ids
            // TODO: replace large beer by custom-sized beer
            return View(new IndexViewModel
            {
                Comments = await _db.Comments.ToArrayAsync(),
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