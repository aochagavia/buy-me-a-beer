using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICommentRepository
    {
        Task<int> Count();
        Task<Comment> Create(Guid paymentId, string nickname, string message);
        Task<Comment> GetByPaymentId(Guid paymentId);
        Task<Comment> GetByStripeSessionId(string sessionId);
        Task<Comment[]> LatestComments();
    }
}