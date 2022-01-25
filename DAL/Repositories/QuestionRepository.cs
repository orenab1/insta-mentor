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

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            return await _mapper
                .ProjectTo<QuestionDto>(_context
                    .Questions
                    .Where(x => x.Id == id))
                .SingleOrDefaultAsync();
        }

        public async Task<int>
        AskQuestionAsync(QuestionFirstSaveDto questionFirstSaveDto)
        {
            Question question = _mapper.Map<Question>(questionFirstSaveDto);

            await _context.Questions.AddAsync(question);

            await _context.SaveChangesAsync();

            await UpdateTagsForQuestion(questionFirstSaveDto.Tags.ToArray(),
            questionFirstSaveDto.AskerId,
            question.Id);

            await UpdateCommunitiesForQuestion(questionFirstSaveDto
                .Communities
                .ToArray(),
            questionFirstSaveDto.AskerId,
            question.Id);

            return question.Id;
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

        public async Task<bool> MarkQuestionAsSolved(int questionId)
        {
            var question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Id == questionId);
            question.IsSolved = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool>
        UpdateTagsForQuestion(TagDto[] newTags, int userId, int questionId)
        {
            Question question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Id == questionId);

            this._tagRepository.AddTagsToDBAndAssignId(ref newTags, userId);

            question.Tags = new List<QuestionsTags>();

            await _context.SaveChangesAsync();

            foreach (TagDto tagDto in newTags)
            {
                question
                    .Tags
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
            Question question =
                await _context
                    .Questions
                    .SingleOrDefaultAsync(x => x.Id == questionId);

            question.Communities = new List<QuestionsCommunities>();

            await _context.SaveChangesAsync();

            foreach (CommunityDto communityDto in newCommunities)
            {
                question
                    .Communities
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
                    Created = DateTime.Now,
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
                    Created = DateTime.Now,
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

        public async Task<IEnumerable<MyQuestionSummaryDto>>
        GetMyQuestionsAsync(int userId)
        {
            var result =
                await _mapper
                    .ProjectTo<MyQuestionSummaryDto>(_context
                        .Questions
                        .Where(q => q.AskerId == userId))
                    .ToListAsync();

            return result
                .OrderBy(myQuestionSummary =>
                    myQuestionSummary.IsActive && !myQuestionSummary.IsSolved
                        ? 0
                        : 1)
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
                        .Where(q => q.IsActive && !q.IsSolved))
                    .ToListAsync();

            if (userTagsIds != null)
            {
                foreach (var questionSummary in result)
                {
                    questionSummary.HasCommonTags =
                        questionSummary
                            .Tags
                            .Select(x => x.Value)
                            .Intersect(userTagsIds)
                            .Any();
                }
            }

            if (userCommunitiesIds != null)
            {
                foreach (var questionSummary in result)
                {
                    questionSummary.HasCommonCommunities =
                        questionSummary
                            .Communities
                            .Select(x => x.Value)
                            .Intersect(userCommunitiesIds)
                            .Any();
                }
            }

            return result
                .OrderBy(questionSummary =>
                    questionSummary.HasCommonCommunities &&
                    questionSummary.HasCommonTags
                        ? 0
                        : questionSummary.HasCommonTags
                            ? 1
                            : questionSummary.HasCommonCommunities ? 2 : 3)
                .ThenByDescending(questionSummary => questionSummary.Created);
        }

        public async Task PublishReview(ReviewDto reviewDto)
        {
            Review review = _mapper.Map<Review>(reviewDto);
            review.Created = DateTime.Now;
            review.IsRevieweeAnswerer = true;
            _context.Reviews.Add (review);
            await _context.SaveChangesAsync();
        }
    }
}
