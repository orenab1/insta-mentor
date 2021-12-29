using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly DataContext _context;

        public QuestionController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("ask-question")]
        public async Task<ActionResult<QuestionDto>> AskQuestion(QuestionDto questionDto)
        {
            _context.Questions.AddAsync(new Question{
                Header=questionDto.Header,
                Body=questionDto.Body
            });

            _context.SaveChangesAsync();
            return Ok("ok!");
        }
        
    }
}