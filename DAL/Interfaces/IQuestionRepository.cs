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

        void AskQuestion(QuestionDto questionDto);

        Task<bool> SaveAllAsync();
    }
}