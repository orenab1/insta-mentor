using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.SignalR;
using DAL;
using DAL.DTOs;
using DAL.DTOs.Partial;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace API.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMailService _mailService;

        private readonly PresenceTracker _tracker;

        private readonly IConfiguration _config;

        private readonly IHubContext<PresenceHub, IPresenceHub> _presenceHub;
 

        public MessagesService(
            IUnitOfWork unitOfWork,
            IMailService mailService,
            PresenceTracker tracker,
            IHubContext<PresenceHub, IPresenceHub> presenceHub,
            IConfiguration config)
        
        {
            this._unitOfWork = unitOfWork;
            this._mailService = mailService;
            this._tracker = tracker;
            this._presenceHub = presenceHub;
            this._config = config;
        }


        public async Task NotifyOffererAskerAcceptedOfferAsync(int offererId,
            AskerAcceptedOfferDto askerAcceptedOfferDto)
        {
             var connections =
                    await _tracker
                        .GetConnectionsForUser(offererId);

                if (connections != null)
                {
                    await _presenceHub
                        .Clients
                        .Clients(connections)
                        .AskerAcceptedOffer(askerAcceptedOfferDto);
                }
        }

        public async Task NotifyAskersOffererLoggedInAsync(int userId)
        {
            IEnumerable<AskerQuestionDTO> askerUsernameQuestionIds =
                await _unitOfWork
                    .QuestionRepository
                    .GetAskerIdsByOffererId(userId);

            foreach (AskerQuestionDTO
                askerUsernameQuestionId
                in
                askerUsernameQuestionIds
            )
            {
                var connections =
                    await _tracker
                        .GetConnectionsForUser(askerUsernameQuestionId
                            .AskerId);

                if (connections != null)
                {
                    await _presenceHub
                        .Clients
                        .Clients(connections)
                        .OffererLoggedIn(askerUsernameQuestionId.QuestionId);
                }
            }
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
                    .GetUserByIdAsync(questionDto.AskerId);

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

            var connections =
                await _tracker.GetConnectionsForUser(asker.Id);

            if (connections != null)
            {
                await _presenceHub
                    .Clients
                    .Clients(connections)
                    .NewOfferReceived(questionId);
            }
        }

        public async Task NotifyNewCommentAsync(int questionId,int currentUserId)
        {
            QuestionDto questionDto =
                await _unitOfWork
                    .QuestionRepository
                    .GetQuestionAsync(questionId);


                      if (currentUserId==questionDto.AskerId)
            {
                return;
            }

            AppUser asker =
                await _unitOfWork
                    .UserRepository
                    .GetUserByIdAsync(questionDto.AskerId);

            if (asker.EmailPrefrence.MyQuestionReceivedNewComment)
            {
                _mailService
                    .SendEmailAsync(new EmailDto {
                        Subject = "Your question just received a new comment!",
                        Body =
                            "A Question of yours just got a new comment! Click here to read it",
                        To = asker.Email
                    });
            }

            var connections =
                await _tracker.GetConnectionsForUser(asker.Id);

            if (connections != null)
            {
                await _presenceHub
                    .Clients
                    .Clients(connections)
                    .NewCommentReceived(questionId);
            }
        }

        public async Task
        InviteToCommunity(int communityId, int userId, string username)
        {
            AppUser user =
                await _unitOfWork.UserRepository.GetUserAsync(username);

            CommunityFullDto community =
                await _unitOfWork
                    .CommunityRepository
                    .GetCommunity(communityId, userId);

            string subject =
                $"Hi, It's {user.UserName}! Check out the community {community.Name} {(community.IsCurrentUserCreator ? "I created" : "")} on ZOOMME.io";

            _mailService
                .SendEmailAsync(new EmailDto {
                    Subject = subject,
                    Body =
                        "\nYou'll be able to ask me and others question there, and get LIVE answers on ZOOM.\n\nCheck it out here!",
                    To = user.Email
                });
        }


        public async Task SendVerificationEmail(string email, string verificationCode)
        {
            
                  _mailService
                .SendEmailAsync(new EmailDto {
                    Subject = "Wellcome to VidCallMe!",
                    Body ="Please click the link below to verify your account <br/><br/>"+ GenerateConfirmEmailLink(email,verificationCode),
                    To = email
                });
        }

        public async Task SendPasswordEmail(string email,string password)
        {
            _mailService
                .SendEmailAsync(new EmailDto {
                    Subject = "Your VidCallMe Password",
                    Body ="Your password is: "+password+ "<br/>-The "+this._config["baseWebUrl"]+" Team",
                    To = email
                });
        }

        public async Task SendPasswordAndVerificationEmail(string email,string verificationCode, string password)
        {
            _mailService
                .SendEmailAsync(new EmailDto {
                    Subject = "Your VidCallMe Password & verification code",
                    Body ="Your password is: "+password+ "<br/>Please click the link below to verify your account <br/><br/>"+ GenerateConfirmEmailLink(email,verificationCode),
                    To = email
                });
        }

        private string GenerateConfirmEmailLink(string email,string verificationCode)
        {
            // var callbackLink = _generator.GetUriByAction(
            //     this._generator,
            //     "register",
            //     "account",
            //     "http"
            // )


            var callbackLink=this._config["baseWebUrl"]+"register?email="+email+"&verificationCode="+verificationCode;
         

            return callbackLink;
        }
    }
}

