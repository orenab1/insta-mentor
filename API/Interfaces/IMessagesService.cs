using System.Security.AccessControl;
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

        void SendVerificationEmail(string email, string verificationCode);

        void SendPasswordEmail(string email,string password);

        void SendPasswordAndVerificationEmail(string email,string verificationCode, string password);

        void SendMeQuestionAskedEmail(string questionHeader,string questionBody);

        void SendMeUserSignedInEmail(string username);

        void NotifyUserHisQuestionAsked(string questionLink,string discordLink,string userEmail);

         void AskFeedback(string askerEmail,string answererUsername,string questionIdOrGuid);
    }
}
