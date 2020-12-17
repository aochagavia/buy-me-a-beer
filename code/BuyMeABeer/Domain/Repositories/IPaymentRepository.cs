using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<int> Count();
        Task<Payment> Create(Guid beerId, string stripeSessionId, int customPrice);
        Task<Payment> GetById(Guid id);
        Task<Payment> GetByStripeSessionId(string sessionId);
    }
}