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

        Task<int> AskQuestionAsync(QuestionEditDto questionDto);

        Task<bool> PostCommentAsync(AddCommentDto commentDto,int userId);

        Task<IEnumerable<QuestionSummaryDto>> GetQuestionsAsync(int[] userTagsIds, int[] userCommunitiesIds);
        
        Task<bool> MakeOfferAsync(int questionId,int userId);

        Task PublishReview(ReviewDto reviewDto);

        Task<IEnumerable<MyQuestionSummaryDto>> GetMyQuestionsAsync(int userId);

        Task<bool> ChangeQuestionActiveStatus(int questionId, bool isActive);

        Task<bool> MarkQuestionAsSolved(int questionId);

        Task DeleteCommentAsync(int commentId,int userId);
        Task DeleteOfferAsync(int offerId, int userId);

        Task<IEnumerable<AskerQuestionDTO>> GetAskerUsernamesByOffererId(int userId);
    }
}