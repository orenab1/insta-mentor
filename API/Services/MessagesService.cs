using System.Threading.Tasks;
using API.Interfaces;
using DAL;
using DAL.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;

namespace API.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMailService _mailService;

        public MessagesService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            this._unitOfWork = unitOfWork;
            this._mailService = mailService;
        }

        public async Task NotifyNewOfferAsync(int questionId)
        {
            QuestionDto questionDto =
                await _unitOfWork
                    .QuestionRepository
                    .GetQuestionAsync(questionId);

            AppUser asker =
                await _unitOfWork
                    .UserRepository
                    .GetUserAsync(questionDto.AskerUsername);

            if (asker.EmailPrefrence.MyQuestionReceivedNewOffer)
            {
                _mailService
                    .SendEmailAsync(new EmailDto {
                        Subject = "New Offer Received!",
                        Body =
                            "A Question of yours just got a new offer! Click here to view it",
                        To = asker.Email
                    });
            }

            // if user is online
            //  send notification
        }
    }
}
