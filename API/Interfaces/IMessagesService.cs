using System.Threading.Tasks;
using DAL.DTOs.Partial;

namespace API.Interfaces
{
    public interface IMessagesService
    {
        // name template: Notify who? what?
        Task NotifyNewOfferAsync(int questionId);

        Task NotifyNewCommentAsync(int questionId);

        Task NotifyAskersOffererLoggedInAsync(int questionId);

        Task NotifyOffererAskerAcceptedOfferAsync(string offererUsername,AskerAcceptedOfferDto askerAcceptedOfferDto);

        Task InviteToCommunity(int communityId, int userId, string username);
    }
}
