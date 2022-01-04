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

        Task<int> AskQuestionAsync(QuestionDto questionDto);

        Task<bool> PostCommentAsync(CommentDto commentDto);

        Task<IEnumerable<QuestionDto>> GetQuestionsAsync();
        Task<bool> MakeOfferAsync(OfferDto offerDto);
    }
}