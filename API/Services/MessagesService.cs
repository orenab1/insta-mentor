using System;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.SignalR;
using DAL;
using DAL.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMailService _mailService;

        private readonly IHubContext<NotificationsHub> _notificationsHubContext;

        private readonly PresenceTracker _tracker;

        public MessagesService(
            IUnitOfWork unitOfWork,
            IMailService mailService,
            IHubContext<NotificationsHub> notificationsHubContext,
            PresenceTracker tracker
        )
        {
            this._unitOfWork = unitOfWork;
            this._mailService = mailService;
            this._notificationsHubContext = notificationsHubContext;
            this._tracker = tracker;
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

            //if (user is online and not turn off is active )
            //  send notification
            var connections =
                await _tracker.GetConnectionsForUser("dave");
              //  await _tracker.GetConnectionsForUser(asker.UserName);

            if (connections != null)
            {
                await _notificationsHubContext
                    .Clients
                    .Clients(connections)
                    .SendAsync("NewOfferReceived",
                    new { questionId = questionId });
            }

            // await _notificationHubsContext.Clients.All.SendAsync(

            // _notificationHubsContext
            //     .NewOfferReceived(asker.UserName, questionId);
        }
    }
}
