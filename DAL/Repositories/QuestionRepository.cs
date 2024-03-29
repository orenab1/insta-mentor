using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.DTOs;
using DAL.DTOs.Full;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

//using System.Runtime.Remoting.Lifetime;
namespace DAL.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        private readonly ITagRepository _tagRepository;

        public QuestionRepository(
            DataContext context,
            IMapper mapper,
            ITagRepository tagRepository
        )
        {
            this._context = context;
            this._mapper = mapper;
            this._tagRepository = tagRepository;
        }

        public async Task<QuestionDto> GetQuestionAsync(string id)
        {
            QuestionDto result =
                await _mapper
                    .ProjectTo<QuestionDto>(_context
                        .Questions
                        .Where(x => x.Guid == id))
                    .SingleOrDefaultAsync();
            
            return result;
        }

         public bool GetHasUserRequestedFeedback(int userId,int questionId)
         {
             return _context.QuestionFeedbackRequestors.Any(x=>x.QuestionId==questionId && x.RequestorId==userId);
         }
         
         public bool WasQuestionFeedbacked(int questionId,int revieweeId)
         {
             return _context.Reviews.Any(x=> x.QuestionId==questionId && x.RevieweeId==revieweeId);
         }


        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            if (id == 0)
            {
                return new QuestionDto();
            }

            QuestionDto result =
                await _mapper
                    .ProjectTo<QuestionDto>(_context
                        .Questions
                        .Where(x => x.Id == id))
                    .SingleOrDefaultAsync();

            // if (result?.LastAnswererUserId.HasValue == true)
            // {
            //     AppUser answerer =
            //         await _context
            //             .Users
            //             .SingleOrDefaultAsync(x =>
            //                 x.Id == result.LastAnswererUserId);

            //     result.LastAnswererUserName = answerer.UserName;
            // }

            // Review review =
            //     await _context
            //         .Reviews?
            //         .FirstOrDefaultAsync(x => x.QuestionId == result.Id);

            // if (review != null)
            // {
            //     AppUser reviewee =
            //         await _context
            //             .Users
            //             .FirstOrDefaultAsync(x => x.Id == review.RevieweeId);
            //     result.RevieweeUsername = reviewee.UserName;
            // }

            return result;
        }

        public async Task<IdAndGuidDTO> AskQuestionAsync(QuestionEditDto questionEditDto, bool isUserRegistered)
        {
            if (questionEditDto.PhotoId == 0)
            {
                questionEditDto.PhotoId = null;
            }
            int? photoID = questionEditDto.PhotoId;
            questionEditDto.PhotoId = null;

            Question question = _mapper.Map<Question>(questionEditDto);

            
            
            if (question.Id == 0)
            {
                question.Created = DateTime.UtcNow;

                if (!isUserRegistered){
                    question.Guid=Guid.NewGuid().ToString();
                }
                question.IsActive = true;
                question.DiscordLink=getDiscordLink();
                question.DiscordLinkId=question.DiscordLink.Id;

                await _context.Questions.AddAsync(question);
                
                await _context.SaveChangesAsync();
                updateDiscordLinkQuestion(question.DiscordLink.Id,question.Id);                
            }
            else
            {
                var questionCreatedFromDb =
                    _context
                        .Questions
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Id == questionEditDto.Id)?
                        .Created;
                question.Created = questionCreatedFromDb.Value;
                _context.Questions.Update (question);
            }
            await _context.SaveChangesAsync();

            if (photoID.HasValue)
            {
                question.PhotoId = photoID;
                await _context.SaveChangesAsync();
            }

            await UpdateTagsForQuestion(questionEditDto.Tags.ToArray(),
            questionEditDto.AskerId,
            question.Id);

            // if (questionEditDto.Communities != null)
            // {
            //     await UpdateCommunitiesForQuestion(questionEditDto
            //         .Communities
            //         .ToArray(),
            //     questionEditDto.AskerId,
            //     question.Id);
            // }

            return new IdAndGuidDTO{
                Id= question.Id,
                Guid=question.Guid
            };
        }

        public async Task<bool>
        ChangeQuestionActiveStatus(int questionId, bool isActive)
        {
            var question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Id == questionId);
            question.IsActive = isActive;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetQuestionIdByOfferId(int offerId)
        {
            Offer offer =
                await _context
                    .Offers
                    .SingleOrDefaultAsync(x => x.Id == offerId);
            return offer.QuestionId;
        }

        public async Task<int> GetOffererUserIdByOfferId(int offerId)
        {
            Offer offer =
                await _context
                    .Offers
                    .SingleOrDefaultAsync(x => x.Id == offerId);

            return offer.OffererId;
        }

        public async Task<bool> MarkQuestionAsSolved(int questionId)
        {
            var question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Id == questionId);
            question.IsSolved = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> MarkQuestionAsSolved(string questionGuid)
        {
            var question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Guid == questionGuid);
            question.IsSolved = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool>
        UpdateTagsForQuestion(TagDto[] allTags, int userId, int questionId)
        {
            int[] allTagsIds = allTags.Select(x => x.Value).ToArray();

            QuestionsTags[] allQuestionsTags =
                _context
                    .QuestionsTags
                    .Where(qt => qt.QuestionId == questionId)
                    .ToArray();

            foreach (QuestionsTags qt in allQuestionsTags)
            {
                _context.Entry(qt).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();

            this._tagRepository.AddTagsToDBAndAssignId(ref allTags, userId);

            foreach (TagDto tagDto in allTags)
            {
                _context
                    .QuestionsTags
                    .Add(new QuestionsTags {
                        QuestionId = questionId,
                        TagId = tagDto.Value
                    });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool>
        UpdateCommunitiesForQuestion(
            CommunityDto[] newCommunities,
            int userId,
            int questionId
        )
        {
            int[] allCommunitiesIds =
                newCommunities.Select(x => x.Value).ToArray();

            QuestionsCommunities[] allQuestionsCommunities =
                _context
                    .QuestionsCommunities
                    .Where(qt => qt.QuestionId == questionId)
                    .ToArray();

            foreach (QuestionsCommunities qt in allQuestionsCommunities)
            {
                _context.Entry(qt).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();

            foreach (CommunityDto communityDto in newCommunities)
            {
                _context
                    .QuestionsCommunities
                    .Add(new QuestionsCommunities {
                        QuestionId = questionId,
                        CommunityId = communityDto.Value
                    });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> MakeOfferAsync(int questionId, int userId)
        {
            _context
                .Offers
                .Add(new Offer {
                    Created = DateTime.UtcNow,
                    OffererId = userId,
                    QuestionId = questionId
                });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool>
        PostCommentAsync(AddCommentDto commentDto, int userId)
        {
            _context
                .Comments
                .Add(new Comment {
                    Text = commentDto.Text,
                    Created = DateTime.UtcNow,
                    QuestionId = commentDto.QuestionId,
                    CommentorId = userId
                });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            Comment comment =
                await _context
                    .Comments
                    .SingleOrDefaultAsync(x => x.Id == commentId);
            if (comment.CommentorId != userId)
                throw new UnauthorizedAccessException();

            comment.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(int offerId, int userId)
        {
            Offer offer =
                await _context
                    .Offers
                    .SingleOrDefaultAsync(x => x.Id == offerId);
            if (offer.OffererId != userId)
                throw new UnauthorizedAccessException();

            _context.Offers.Remove (offer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<QuestionSummaryDto>>
        GetMyQuestionsAsync(int userId)
        {
            var result =
                await _mapper
                    .ProjectTo<QuestionSummaryDto>(_context
                        .Questions
                        .Where(q => q.AskerId == userId))
                    .ToListAsync();

            return result
                .OrderBy(myQuestionSummary =>
                    !myQuestionSummary.IsSolved ? 0 : 1)
                .ThenByDescending(myQuestionSummary =>
                    myQuestionSummary.Created);
        }

        public async Task<IEnumerable<QuestionSummaryDto>>
        GetQuestionsAsync(int[] userTagsIds, int[] userCommunitiesIds)
        {
            var result =
                await _mapper
                    .ProjectTo<QuestionSummaryDto>(_context
                        .Questions
                        .Where(q => !q.IsSolved && q.IsActive))
                    .ToListAsync();

            // if (userTagsIds != null)
            // {
            //     foreach (var questionSummary in result)
            //     {
            //         questionSummary.HasCommonTags =
            //             questionSummary
            //                 .Tags
            //                 .Select(x => x.Value)
            //                 .Intersect(userTagsIds)
            //                 .Any();
            //     }
            // }

            return result
                .OrderBy(questionSummary => questionSummary.Created);
        }

        public async Task PublishReview(ReviewDto reviewDto)
        {
            Review review = _mapper.Map<Review>(reviewDto);
            review.Created = DateTime.UtcNow;
            review.IsRevieweeAnswerer = true;
            _context.Reviews.Add (review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AskerQuestionDTO>>
        GetAskerIdsByOffererId(int userId)
        {
            return _context
                .Offers
                .Where(o => o.OffererId == userId)
                .Select(offer => offer.Question)
                .Where(q => q.IsActive)
                .Select(q =>
                    new AskerQuestionDTO {
                        AskerId = q.Asker.Id,
                        QuestionId = q.Id
                    });
        }

        public async Task<bool>
        UpdateQuestionLastOfferer(int questionId, int userId)
        {
            Question question =
                await _context
                    .Questions
                    .FirstOrDefaultAsync(q => q.Id == questionId);
            question.LastAnswererUserId = userId;
            return await _context.SaveChangesAsync() > 0;
        }

        public EventDto[] GetNextEvents()
        {
            var result =
                _mapper
                    .ProjectTo<EventDto>(_context
                        .Events
                        .Where(e => e.Time > DateTime.UtcNow.AddHours(-15))).ToList();

            var topicIdMinTime =
                result
                    .GroupBy(e => e.TopicIdentifier)
                    .Select(g =>
                        new { topic = g.Key, time = g.Min(c => c.UtcTime) });

            return result
                .Where(e =>
                    topicIdMinTime
                        .Any(t =>
                            t.topic == e.TopicIdentifier &&
                            t.time == e.UtcTime))
                .ToArray();
        }

        public string GetQuestionDiscordLink(int questionId){
            return _context.Questions
                .Include(q=>q.DiscordLink)
                .Single(q=>q.Id==questionId)
                .DiscordLink.Link;
        }

        public string GetAskerEmail(string questionIdOrGuid){
            return _context.Questions
                .Include(q=>q.Asker)
                .Single(q=>q.Id.ToString()==questionIdOrGuid || q.Guid==questionIdOrGuid)
                .Asker.Email;
        }

        public void RemoveDiscordLink(string questionIdOrGuid){
            Question question= _context.Questions
                .Include(q=>q.DiscordLink)
                .Single(q=>q.Id.ToString()==questionIdOrGuid || q.Guid==questionIdOrGuid);

            question.DiscordLink=null;
            _context.SaveChanges();
        }

        public void MarkFeedbackRequested(string questionIdOrGuid,int userId){

            Question question= _context.Questions
                .Include(q=>q.Asker)
                .Single(q=>q.Id.ToString()==questionIdOrGuid || q.Guid==questionIdOrGuid);

            _context.QuestionFeedbackRequestors.Add(new QuestionFeedbackRequestor{
                Question=question,
                RequestorId=userId
            });

            _context.SaveChanges();
        }
        private DiscordLink getDiscordLink(){
            return  _context.DiscordLinks.FirstOrDefault(x=>x.Question==null);
            // result.QuestionId=questionId;
            // _context.SaveChanges();
        }

        private void updateDiscordLinkQuestion(int discordLinkId, int questionId)
        {
            DiscordLink link=_context.DiscordLinks.Single(x=> x.Id==discordLinkId);
            link.QuestionId=questionId;
            _context.SaveChanges();
        }
    }
}
