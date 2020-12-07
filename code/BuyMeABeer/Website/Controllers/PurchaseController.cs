using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Website.Database;
using Website.Database.Entities;

namespace Website.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly WebsiteDbContext _db;

        public PurchaseController(WebsiteDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        // TODO: the controller will return an empty http response if a GET is issued... Can we do something more user-friendly
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            // TODO: extract logic to a service so we can actually test it
            var purchase = new Purchase
            {
                BeerId = Guid.Empty, // TODO: use real guids for this stuff
            };
            var comment = new Comment
            {
                Purchase = purchase,
                Nickname = "Dummy", // TODO: get this from POST
                Message = "The answer is 42", // TODO: get this from POST
            };

            _db.Add(comment);
            await _db.SaveChangesAsync();

            return View();
        }
    }
}
