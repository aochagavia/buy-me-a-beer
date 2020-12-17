using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Database;

namespace Website.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WebsiteDbContext _db;

        public CommentRepository(WebsiteDbContext db)
        {
            _db = db;
        }

        public async Task<Comment> Create(Guid paymentId, string nickname, string message)
        {
            var comment = new Comment
            {
                PaymentId = paymentId,
                Nickname = nickname,
                Message = message,
                CreatedUtc = DateTimeOffset.UtcNow,
            };

            _db.Add(comment);
            await _db.SaveChangesAsync();

            return comment;
        }

        public Task<Comment> GetByStripeSessionId(string sessionId)
        {
            return _db.Comments
                .Where(c => c.Payment.StripeSessionId == sessionId)
                .FirstOrDefaultAsync();
        }

        public Task<Comment> GetByPaymentId(Guid paymentId)
        {
            return _db.Comments
                .Where(c => c.PaymentId == paymentId)
                .FirstOrDefaultAsync();
        }

        public Task<Comment[]> LatestComments()
        {
            return _db.Comments
                .Include(c => c.Payment)
                .OrderByDescending(c => c.CreatedUtc)
                .ToArrayAsync();
        }

        public Task<int> Count()
        {
            return _db.Comments.CountAsync();
        }
    }
}
