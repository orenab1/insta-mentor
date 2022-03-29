using System.Threading.Tasks;
using DAL.DTOs.Partial;
using DAL.DTOs.Full;

namespace API.Interfaces
{
    public interface IPresenceHub
    {
         Task NewOfferReceived(int questionId);

         Task NewCommentReceived(int questionId);
         Task UserIsOnline(string username);

         Task GetOnlineUsers(string[] onlineUsers);

         Task GetOnlineUsersWithTimes(UserConnectedDurationDto[] onlineUsers);

         Task UserIsOffline(string username);

         Task OffererLoggedIn(int questionId);

         Task AskerAcceptedOffer(AskerAcceptedOfferDto askerAcceptedOfferDto);
    }
}