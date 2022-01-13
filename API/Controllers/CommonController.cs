using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using API.Extensions;



namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IUserRepository _userRepository;

        public CommonController(ICommonRepository commonRepository, IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            this._commonRepository = commonRepository;
        }



        [HttpGet("get-tags")]
        public async Task<ActionResult> GetTags()
        {
            var userName = User.GetUsername();
            var user = await _userRepository.GetUserAsync(User.GetUsername());
            return Ok(await _commonRepository.GetTagsByCreatorOrApproved(user.Id));
        }

    }
}