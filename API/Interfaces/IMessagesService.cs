using System.Threading.Tasks;
using DAL.DTOs.Partial;

namespace API.Interfaces
{
    public interface IMessagesService
    {
        // name template: Notify who? what?
        Task NotifyNewOfferAsync(int questionId);

        Task NotifyNewCommentAsync(int questionId,int currentUserId);

        Task NotifyAskersOffererLoggedInAsync(int questionId);

        Task NotifyOffererAskerAcceptedOfferAsync(int offererId,AskerAcceptedOfferDto askerAcceptedOfferDto);

        Task InviteToCommunity(int communityId, int userId, string username);
    }
}
