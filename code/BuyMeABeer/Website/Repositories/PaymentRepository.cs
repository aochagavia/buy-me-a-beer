using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Website.Database;

namespace Website.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly WebsiteDbContext _db;

        public PaymentRepository(WebsiteDbContext db)
        {
            _db = db;
        }

        public async Task<Payment> Create(Guid beerId, string stripeSessionId, int customPrice)
        {
            var payment = new Payment
            {
                BeerId = beerId,
                StripeSessionId = stripeSessionId,
                Amount = customPrice,
            };

            _db.Add(payment);
            await _db.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetById(Guid id)
        {
            return await _db.Payments
                .Include(payment => payment.Comment)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> GetByStripeSessionId(string sessionId)
        {
            return await _db.Payments
                .Include(payment => payment.Comment)
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);
        }

        public Task<int> Count()
        {
            return _db.Payments.CountAsync();
        }
    }
}
