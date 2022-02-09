using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using DAL.DTOs;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMessagesService _messagesService;

        public CommunitiesController(
            IUnitOfWork unitOfWork,
            IMessagesService messagesService
        )
        {
            this._unitOfWork = unitOfWork;
            this._messagesService = messagesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommunityFullDto>>
        GetCommunitiesSummaries()
        {
            return Ok(_unitOfWork
                .CommunityRepository
                .GetCommunitiesSummaries(User.GetUserId()));
        }

        [HttpDelete]
        [ActionName("delete")]
        public async Task<IActionResult> Delete(int communityId)
        {
            if (await CanDeleteCommunity(communityId) == false)
                return Unauthorized();

            await _unitOfWork.CommunityRepository.DeleteCommunity (communityId);

            return NoContent();
        }

        [HttpPut]
        [ActionName("leave")]
        public async Task<ActionResult> Leave(int communityId)
        {
            await _unitOfWork
                .CommunityRepository
                .LeaveCommunity(communityId, User.GetUserId());

            return NoContent();
        }

        [HttpPut]
        [ActionName("join")]
        public async Task<ActionResult> Join(int communityId)
        {
            await _unitOfWork
                .CommunityRepository
                .JoinCommunity(communityId, User.GetUserId());

            return NoContent();
        }

        [HttpPost]
        [ActionName("invite")]
        public async Task<ActionResult> Invite(int communityId)
        {
            await _unitOfWork
                .CommunityRepository
                .JoinCommunity(communityId, User.GetUserId());

            return NoContent();
        }

        [HttpPost]
        [ActionName("create")]
        public async Task<ActionResult> Create(AddCommunityDto addCommunityDto)
        {
            await _unitOfWork
                .CommunityRepository
                .CreateCommunity(addCommunityDto, User.GetUserId());
            return NoContent();
        }

        private async Task<bool>
        CanAddCommunity(AddCommunityDto addCommunity)
        {
            DateTime? lastCreatedCommunityByUser =
                await _unitOfWork
                    .CommunityRepository
                    .LastCreatedCommunity(User.GetUserId());

            if (
                (
                lastCreatedCommunityByUser.HasValue &&
                (DateTime.Now - lastCreatedCommunityByUser.Value).TotalHours <
                24
                )
            )
            {
                return false;
            }

            if (await _unitOfWork
                    .CommunityRepository
                    .IsCommunityNameExists(addCommunity.Name))
                    {
                        return false;
                    }

            

            return true;
        }

        private async Task<bool> CanDeleteCommunity(int communityId)
        {
            var numOfUsersInCommunity =
                _unitOfWork
                    .CommunityRepository
                    .GetNumOfUsersInCommunity(communityId);

            if (numOfUsersInCommunity > 5) return false;

            var community =
                _unitOfWork
                    .CommunityRepository
                    .GetCommunitiesSummaries(User.GetUserId())
                    .SingleOrDefault(x => x.Id == communityId);

            if (
                community == null ||
                !community.IsCurrentUserMember ||
                !community.IsCurrentUserCreator
            ) return false;
            return true;
        }
    }
}
