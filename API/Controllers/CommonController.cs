using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.Extensions;
using DAL.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommonController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("get-tags")]
        public async Task<ActionResult> GetTags()
        {
            var userName = User.GetUsername();
            var user = await _unitOfWork.UserRepository.GetUserAsync(User.GetUsername());
            return Ok(await _unitOfWork.TagRepository.GetTagsByCreatorOrApproved(user.Id));
        }

        [HttpGet("get-communities")]
        public async Task<ActionResult> GetCommunities()
        {
            var userName = User.GetUsername();
            return Ok(await _unitOfWork.CommunityRepository.GetCommunities());
        }
    }
}