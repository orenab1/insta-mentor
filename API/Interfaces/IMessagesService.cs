using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessagesService
    {
        Task NotifyNewOfferAsync(int questionId);

        Task NotifyNewCommentAsync(int questionId);

        Task NotifyAskersOffererLoggedInAsync(int questionId);
    }
}