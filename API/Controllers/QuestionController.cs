using Microsoft.AspNetCore.Mvc;
using DAL.DTOs;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using API.Interfaces;
using API.Extensions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;

        public QuestionsController(IQuestionRepository questionRepository, IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            this._questionRepository = questionRepository;
        }

        [HttpPost("ask-question")]
        public async Task<ActionResult<QuestionDto>> AskQuestion(QuestionDto questionDto)
        {
            int id = await _questionRepository.AskQuestionAsync(questionDto);

            return Ok(id);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            return await _questionRepository.GetQuestionAsync(id);
        }

        [HttpPost("post-comment")]
        public async Task<ActionResult<CommentDto>> PostComment(CommentDto commentDto)
        {
            commentDto.CommentorUsername = User.GetUsername();
            await _questionRepository.PostCommentAsync(commentDto);

            //_questionRepository.SaveAllAsync();
            return Ok();
        }

        [HttpPost("make-offer")]
        public async Task<ActionResult> MakeOffer([FromBody]int questionId)
        {
            OfferDto offer = new OfferDto
            {
                Username = User.GetUsername(),
                QuestionId = questionId
            };

            await _questionRepository.MakeOfferAsync(offer);

            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            return Ok(await _questionRepository.GetQuestionsAsync());
        }

    }
}