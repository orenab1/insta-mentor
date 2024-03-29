using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using API.Services;
using API.Settings;
using API.SignalR;
using AutoMapper;
using DAL;
using DAL.DTOs;
using DAL.DTOs.Full;
using DAL.DTOs.Partial;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;
using Serilog.Sinks.MSSqlServer;

// using RestSharpAutomation.HelperClass.Request;
// using RestSharpAutomation.ReportAttribute;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IPhotoService _photoService;

        private readonly IMapper _mapper;

        private readonly IMessagesService _messagesService;

        private readonly IZoomService _zoomService;

        private readonly ILogger _logger;

        public QuestionsController(
            IUnitOfWork unitOfWork,
            IPhotoService photoService,
            IMapper mapper,
            IMessagesService messagesService,
            IZoomService zoomService,
            ILogger logger
        )
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
            this._photoService = photoService;
            this._mapper = mapper;
            this._messagesService = messagesService;
            this._zoomService = zoomService;
        }

        [HttpPost]
        [ActionName("accept-offer")]
        public async Task<ActionResult<AcceptedOfferDto>>
        AcceptOffer(AcceptOfferDto acceptOfferDto)
        {
            int questionId =
                await _unitOfWork
                    .QuestionRepository
                    .GetQuestionIdByOfferId(acceptOfferDto.OfferId);

            QuestionDto questionDto =
                await _unitOfWork
                    .QuestionRepository
                    .GetQuestionAsync(questionId);

            int offererId =
                await _unitOfWork
                    .QuestionRepository
                    .GetOffererUserIdByOfferId(acceptOfferDto.OfferId);

            string meetingUrl =
                await this._zoomService.GetMeetingUrl(questionDto.Header);

            AskerAcceptedOfferDto askerAcceptedOfferDto =
                new AskerAcceptedOfferDto {
                    OffererUserId = offererId,
                    QuestionHeader = questionDto.Header,
                    QuestionBody = questionDto.Body,
                    AskerUserId = questionDto.AskerId,
                    MeetingUrl = meetingUrl,
                    QuestionId = questionId,
                    AskerUsername = questionDto.AskerUsername
                };

            AppUser offererAppUser =
                await _unitOfWork.UserRepository.GetUserByIdAsync(offererId);

            string offererUsername = offererAppUser.UserName;

            await _messagesService
                .NotifyOffererAskerAcceptedOfferAsync(offererId,
                askerAcceptedOfferDto);

            await _unitOfWork
                .QuestionRepository
                .UpdateQuestionLastOfferer(questionId, offererId);
            return Ok(new AcceptedOfferDto { MeetingUrl = meetingUrl });

            // var tokenHandler =
            //     new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            // var now = DateTime.UtcNow;
            // var apiSecret = _zoomSettings.APISecret;
            // byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            // var tokenDescriptor =
            //     new SecurityTokenDescriptor {
            //         Issuer = _zoomSettings.APIKey,
            //         Expires = now.AddSeconds(600),
            //         SigningCredentials =
            //             new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
            //                 SecurityAlgorithms.HmacSha256)
            //     };
            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // var tokenString = tokenHandler.WriteToken(token);
            // RestClient client =
            //     new RestClient("https://api.zoom.us/v2/users/me/meetings");

            // var request = new RestRequest();

            // request.RequestFormat = DataFormat.Json;
            // request
            //     .AddJsonBody(new {
            //         topic = "Meeting with Ussain134",
            //         type = "1",
            //         settings = new { join_before_host = true }
            //     });
            // request
            //     .AddHeader("authorization",
            //     String.Format("Bearer {0}", tokenString));
            // var restResponse = await client.PostAsync<object>(request);
            // var responseAsJObject = JObject.Parse(restResponse.ToString());

            // string meetingUrl = responseAsJObject["start_url"].ToString();
        }

        [HttpPost]
        [ActionName("ask-question")]
        public async Task<ActionResult<string>>
        AskQuestion(QuestionEditDto questionEditSaveDto)
        {
            bool isUserRegistered=User.GetUserId() != 0;

            if (!isUserRegistered)
            {
                AppUser user =
                    await _unitOfWork
                        .UserRepository
                        .GetUserByEmailAsync(questionEditSaveDto.UserEmail, false);

                if (user == null)
                {
                    user =
                        await _unitOfWork
                            .AccountRepository
                            .CreateFakeUserAsync(questionEditSaveDto.UserEmail);
                }

                questionEditSaveDto.AskerId = user.Id;


            }
            else
            {
                questionEditSaveDto.AskerId = User.GetUserId();
            }

            

          IdAndGuidDTO idAndGuid =
                (await _unitOfWork
                    .QuestionRepository
                    .AskQuestionAsync(questionEditSaveDto,isUserRegistered));

            string discordLink=_unitOfWork
                    .QuestionRepository
                    .GetQuestionDiscordLink(idAndGuid.Id);


            if (!isUserRegistered){
                _messagesService.NotifyUserHisQuestionAsked(idAndGuid.Guid,discordLink,questionEditSaveDto.UserEmail);
            }

            _messagesService.SendMeQuestionAskedEmail(questionEditSaveDto.Header,questionEditSaveDto.Body);
            return Ok(idAndGuid.GetIdOrGuid());
        }

        [Authorize]
        [HttpPut]
        [ActionName("change-question-active-status")]
        public async Task<ActionResult>
        ChangeQuestionActiveStatus(ChangeQuestionActiveStatusDto dto)
        {
            await _unitOfWork
                .QuestionRepository
                .ChangeQuestionActiveStatus(dto.QuestionId, dto.IsActive);
            return NoContent();
        }

        [HttpPut]
        [ActionName("mark-question-as-solved")]
        public async Task<ActionResult>
        MarkQuestionAsSolved(QuestionIdOrGuidDto questionIdOrGuid)
        {
            int questionId;

            if (int.TryParse(questionIdOrGuid.QuestionIdOrGuid,out questionId)){
                await _unitOfWork
                    .QuestionRepository
                    .MarkQuestionAsSolved(questionId);
            }
            else {
                 await _unitOfWork
                    .QuestionRepository
                    .MarkQuestionAsSolved(questionIdOrGuid.QuestionIdOrGuid);
            }

            _unitOfWork
                    .QuestionRepository
                    .RemoveDiscordLink(questionIdOrGuid.QuestionIdOrGuid);
           
            return Ok();
        }


        [HttpPost]
        [ActionName("request-feedback")]
        public async Task<ActionResult>
        RequestFeedback(QuestionIdOrGuidDto questionIdOrGuid)
        {
            int userId=User.GetUserId();
            if (userId==0){
                throw new InvalidOperationException();
            }

            int questionId;
             string askerEmail=_unitOfWork.QuestionRepository.GetAskerEmail(questionIdOrGuid.QuestionIdOrGuid);

            _unitOfWork.QuestionRepository.MarkFeedbackRequested(questionIdOrGuid.QuestionIdOrGuid,userId);
            
            _messagesService.AskFeedback(askerEmail, User.GetUsername(),userId,questionIdOrGuid.QuestionIdOrGuid);
           
            return Ok();
        }

        [HttpGet("{id}")]
        [ActionName("get-question")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            QuestionDto result= await _unitOfWork.QuestionRepository.GetQuestionAsync(id);
            int userId=User.GetUserId();

            
            result.HasCurrentUserRequestedFeedback=
                userId !=0 && _unitOfWork.QuestionRepository.GetHasUserRequestedFeedback(userId,id);
        
            result.WasReviewed=_unitOfWork.QuestionRepository.WasQuestionFeedbacked(id,userId);

            return result;
        }

        [HttpGet("{guid}")]
        [ActionName("get-question-by-guid")]
        public async Task<ActionResult<QuestionDto>> GetQuestionByGuid(string guid)
        {
            return await _unitOfWork.QuestionRepository.GetQuestionAsync(guid);
        }

        [HttpPost]
        [ActionName("post-comment")]
        public async Task<ActionResult> PostComment(AddCommentDto commentDto)
        {
            if (
                await _unitOfWork
                    .QuestionRepository
                    .PostCommentAsync(commentDto, User.GetUserId())
            )
            {
                await _messagesService
                    .NotifyNewCommentAsync(commentDto.QuestionId,
                    User.GetUserId());
            }

            return Ok();
        }

        [HttpPost]
        [ActionName("make-offer")]
        public async Task<ActionResult> MakeOffer(OfferDto offerDto)
        {
            if (
                await _unitOfWork
                    .QuestionRepository
                    .MakeOfferAsync(offerDto.QuestionId, User.GetUserId())
            )
            {
                await _messagesService.NotifyNewOfferAsync(offerDto.QuestionId);
            }

            return Ok();
        }

        [HttpGet]
        [ActionName("my-questions")]
        public async Task<ActionResult<IEnumerable<MyQuestionSummaryDto>>>
        GetMyQuestions()
        {
            return Ok(await _unitOfWork
                .QuestionRepository
                .GetMyQuestionsAsync(User.GetUserId()));
        }

        [HttpGet()]
        [ActionName("questions")]
        public async Task<ActionResult<IEnumerable<QuestionSummaryDto>>>
        GetQuestions()
        {
            var user =
                await _unitOfWork.UserRepository.GetUserAsync(User.GetUserId());
            int[] userTagsIds = user?.Tags?.Select(x => x.Value).ToArray();
            int[] userCommunitiesIds =
                user?.Communities?.Select(x => x.Value).ToArray();

            var questions =
                (
                await _unitOfWork
                    .QuestionRepository
                    .GetQuestionsAsync(userTagsIds, userCommunitiesIds)
                ).ToList();

            UserConnectedDurationDto[] usersDuration =
                _unitOfWork.UserRepository.GetOnlineUsersWithTimes();

            for (int i = 0; i < questions.Count(); i++)
            {
                UserConnectedDurationDto duration =
                    usersDuration
                        .SingleOrDefault(x =>
                            x.Username == questions[i].AskerUsername);
                int onlineAge =
                    duration == null ? 0 : Math.Max(duration.SecondsElapsed, 1);

                questions[i].OnlineAgeSeconds =
                    Math.Min(onlineAge, questions[i].AgeInSeconds);
            }

            return Ok(await Task.FromResult(questions));
        }

        [HttpPost]
        [ActionName("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo =
                new Photo {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

            var photoId = await _unitOfWork.CommonRepository.AddPhoto(photo);

            if (photoId != 0)
            {
                return CreatedAtRoute("GetPhoto",
                new { controller = "common", photoId = photoId },
                value: photo);
            }

            return BadRequest("Problem addding photo");
        }

        [HttpPost]
        [ActionName("publish-review")]
        public async Task<ActionResult> PublishReview(ReviewDto reviewDto)
        {
            await _unitOfWork.QuestionRepository.PublishReview(reviewDto);
            return Ok();
        }

        [HttpDelete]
        [ActionName("delete-comment")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            await _unitOfWork
                .QuestionRepository
                .DeleteCommentAsync(commentId, User.GetUserId());

            return NoContent();
        }

        [HttpDelete]
        [ActionName("delete-offer")]
        public async Task<ActionResult> DeleteOffer(int offerId)
        {
            await _unitOfWork
                .QuestionRepository
                .DeleteOfferAsync(offerId, User.GetUserId());

            return NoContent();
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto()
        {
            var user =
                await _unitOfWork
                    .UserRepository
                    .GetUserAsync(User.GetUsername());

            var photo = user.Photo;

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result =
                    await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            user.Photo = null;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

        [HttpGet()]
        [ActionName("events")]
        public ActionResult<IEnumerable<EventDto>> GetEvents()
        {
            return Ok(_unitOfWork.QuestionRepository.GetNextEvents());
        }

        
    }
}
