using System.Threading.Tasks;

namespace Domain.Integration
{
    public interface IStripeSessionService
    {
        Task<string> CreateStripeSession(string itemDescription, int itemPrice);
    }
}
