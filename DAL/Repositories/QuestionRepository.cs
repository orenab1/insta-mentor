using System.Configuration;
using DAL.Interfaces;
using DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DAL.DTOs;
using System.Linq;
using System;
using System.Security.AccessControl;
//using System.Runtime.Remoting.Lifetime;

namespace DAL.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;

        public QuestionRepository(DataContext context, IMapper mapper, ITagRepository tagRepository)
        {
            this._context = context;
            this._mapper = mapper;
            this._tagRepository = tagRepository;
        }

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            return await _mapper
                .ProjectTo<QuestionDto>(_context.Questions.Where(x => x.Id == id))
                .SingleOrDefaultAsync();
        }

        public async Task<int> AskQuestionAsync(QuestionFirstSaveDto questionFirstSaveDto)
        {
            Question question = _mapper.Map<Question>(questionFirstSaveDto);

            await _context.Questions.AddAsync(question);

            await _context.SaveChangesAsync();

            await UpdateTagsForQuestion(questionFirstSaveDto.Tags.ToArray(), questionFirstSaveDto.AskerId, question.Id);

            await UpdateCommunitiesForQuestion(questionFirstSaveDto.Communities.ToArray(),questionFirstSaveDto.AskerId,question.Id);
            //this._tagRepository.AddTagsToDBAndAssignId(ref questionFirstSaveDto.Tags, questionFirstSaveDto.AskerId);



            // questionFirstSaveDto.Tags.Where(x => x.Value == 0)

            // foreach (TagDto tag in questionFirstSaveDto.Tags.Where(x => x.Value == 0))
            // {
            //     _context.Tags.Add(
            //         new Tag
            //     )
            // }





            // foreach (TagDto tag in questionFirstSaveDto.Tags)
            // {
            //     _context.QuestionsTags.Add(
            //         new QuestionsTags
            //         {
            //             QuestionId = questionFirstSaveDto.Id,
            //             TagId = tag.Value
            //         }
            //     );
            // };


            return questionFirstSaveDto.Id;
        }


        public async Task<bool> UpdateTagsForQuestion(TagDto[] newTags, int userId, int questionId)
        {
            Question question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == questionId);

            this._tagRepository.AddTagsToDBAndAssignId(ref newTags, userId);

            question.Tags = new List<QuestionsTags>();

            await _context.SaveChangesAsync();

            foreach (TagDto tagDto in newTags)
            {
                question.Tags.Add(new QuestionsTags
                {
                    QuestionId = questionId,
                    TagId = tagDto.Value
                });
            };

           return await _context.SaveChangesAsync()>0;            
        }


        public async Task<bool> UpdateCommunitiesForQuestion(CommunityDto[] newCommunities, int userId, int questionId)
        {
            Question question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == questionId);

            question.Communities = new List<QuestionsCommunities>();

            await _context.SaveChangesAsync();

            foreach (CommunityDto communityDto in newCommunities)
            {
                question.Communities.Add(new QuestionsCommunities
                {
                    QuestionId = questionId,
                    CommunityId = communityDto.Value
                });
            };

           return await _context.SaveChangesAsync()>0;            
        }

        public async Task<bool> MakeOfferAsync(OfferDto offerDto)
        {
            var question = _context.Questions.SingleOrDefault(x => x.Id == offerDto.QuestionId);
            var user = _context.Users.SingleOrDefault(x => x.UserName == offerDto.Username);

            if (question.Offers == null)
            {
                question.Offers = new List<Offer>();
            }
            question.Offers.Add(
                new Offer
                {
                    Text = "",
                    Created = DateTime.Now,
                    Question = question,
                    Offerer = user
                }
            );


            _context.Entry(question).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> PostCommentAsync(CommentDto commentDto)
        {
            var question = _context.Questions.SingleOrDefault(x => x.Id == commentDto.QuestionId);
            var user = _context.Users.SingleOrDefault(x => x.UserName == commentDto.CommentorUsername);

            if (question.Comments == null)
            {
                question.Comments = new List<Comment>();
            }
            question.Comments.Add(
                new Comment
                {
                    Text = commentDto.Text,
                    Created = DateTime.Now,
                    Question = question,
                    Commentor = user
                }
            );


            _context.Entry(question).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<QuestionSummaryDto>> GetQuestionsAsync()
        {
            return await _mapper
                .ProjectTo<QuestionSummaryDto>(_context.Questions)
                .ToListAsync();
        }


        public async Task PublishReview(ReviewDto reviewDto)
        {
            Review review = _mapper.Map<Review>(reviewDto);
            review.Created = DateTime.Now;
            review.IsRevieweeAnswerer = true;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}