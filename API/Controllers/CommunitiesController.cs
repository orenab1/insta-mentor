using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using DAL.DTOs;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommunitiesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet()]
        [ActionName("all-communities")]
        public async Task<ActionResult<IEnumerable<CommunitySummaryDto>>>
        GetCommunitiesSummaries()
        {   
             return Ok(await _unitOfWork.CommunityRepository.GetCommunitiesSummaries());
        }
    }
}
