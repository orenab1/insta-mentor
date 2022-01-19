using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IQuestionRepository
    {
        Task<QuestionDto> GetQuestionAsync(int id);

        Task<int> AskQuestionAsync(QuestionFirstSaveDto questionDto);

        Task<bool> PostCommentAsync(CommentDto commentDto);

        Task<IEnumerable<QuestionSummaryDto>> GetQuestionsAsync(int[] userTagsIds, int[] userCommunitiesIds);
        
        Task<bool> MakeOfferAsync(OfferDto offerDto);

        Task PublishReview(ReviewDto reviewDto);
    }
}