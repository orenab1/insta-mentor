using Microsoft.AspNetCore.Mvc;
using DAL.DTOs;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            this._questionRepository = questionRepository;
        }

        [HttpPost("ask-question")]
        public async Task<ActionResult<QuestionDto>> AskQuestion(QuestionDto questionDto)
        {
            int id = await _questionRepository.AskQuestion(questionDto);

            //_questionRepository.SaveAllAsync();
            return Ok(id);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            // if (id==0)
            // {
            //     return await new QuestionDto();
            // }
            return await _questionRepository.GetQuestionAsync(id);
        }

    }
}