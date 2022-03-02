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
            return Ok(await _unitOfWork.TagRepository.GetTagsByCreatorOrApproved(User.GetUserId()));
        }


         [HttpGet(Name = "GetPhoto")]
         [ActionName("get-photo")]
        public async Task<ActionResult<string>> GetPhoto(int photoId)
        {
            return Ok(await _unitOfWork.CommonRepository.GetPhotoUrl(photoId)); 
        }

       
    }
}