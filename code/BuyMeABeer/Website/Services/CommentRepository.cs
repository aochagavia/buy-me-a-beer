using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Website.Database;
using Website.Database.Entities;

namespace Website.Services
{
    public class CommentRepository
    {
        private readonly WebsiteDbContext _db;

        public CommentRepository(WebsiteDbContext db)
        {
            _db = db;
        }

        public Task<Comment[]> LatestComments()
        {
            return _db.Comments
                .OrderByDescending(c => c.CreatedUtc)
                .ToArrayAsync();
        }
    }
}
