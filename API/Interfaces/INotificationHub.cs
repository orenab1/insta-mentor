using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface INotificationHub
    {
        Task NewOfferReceived(string askerUsername, int questionId);
    }
}