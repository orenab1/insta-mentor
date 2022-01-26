using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface INotificationsHub
    {
        Task NewOfferReceived(string askerUsername, int questionId);
    }
}