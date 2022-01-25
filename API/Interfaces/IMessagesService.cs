using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessagesService
    {
        Task NotifyNewOfferAsync(int questionId);
    }
}