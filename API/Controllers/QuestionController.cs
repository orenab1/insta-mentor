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
using AutoMapper;

using Microsoft.AspNetCore.Http;
using System.Configuration;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionRepository questionRepository,
        IUserRepository userRepository,
        IPhotoService photoService,
        IMapper mapper)
        {
            this._userRepository = userRepository;
            this._questionRepository = questionRepository;
            this._photoService = photoService;
            this._mapper = mapper;
        }

        [HttpPost("ask-question")]
        public async Task<ActionResult<int>> AskQuestion(QuestionDto questionDto)
        {
            AppUser user= await _userRepository.GetUserAsync(User.GetUsername());
            questionDto.AskerId =user.Id;
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
        public async Task<ActionResult> MakeOffer([FromBody] int questionId)
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



        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            // var k = HttpContext.Request.Query["page"].ToString();
            var user = await _userRepository.GetUserAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.Photo = photo;

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",
                new { username = user.UserName },
                _mapper.Map<PhotoDto>(photo));
            }


            return BadRequest("Problem addding photo");
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto()
        {
            var user = await _userRepository.GetUserAsync(User.GetUsername());

            var photo = user.Photo;

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photo = null;

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}