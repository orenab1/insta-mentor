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
    [Route("api/[controller]/[action]")]
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
        [ActionName("all-communities")]
        public ActionResult<IEnumerable<CommunityFullDto>> GetCommunities()
        {
            return Ok(_unitOfWork
                .CommunityRepository
                .GetCommunitiesFull(User.GetUserId()));
        }

        [HttpDelete]
        [ActionName("delete")]
        public async Task<IActionResult> Delete(int communityId)
        {
            if (await CanDeleteCommunity(communityId) == false)
                return Unauthorized();

            await _unitOfWork.CommunityRepository.DeleteCommunity(communityId);

            return NoContent();
        }

        [HttpPut]
        [ActionName("leave")]
        public async Task<ActionResult> Leave(int communityId)
        {
            var success =
                await _unitOfWork
                    .CommunityRepository
                    .LeaveCommunity(communityId, User.GetUserId());

            if (success) return Ok();
            return BadRequest("Failed to leave");
        }

        [HttpPut]
        [ActionName("join")]
        public async Task<ActionResult> Join(int communityId)
        {
            var success =
                await _unitOfWork
                    .CommunityRepository
                    .JoinCommunity(communityId, User.GetUserId());

            if (success) return Ok();
            return BadRequest("Failed to join");
        }

        [HttpPut]
        [ActionName("invite")]
        public async Task<ActionResult> Invite(int communityId)
        {
            await _messagesService
                .InviteToCommunity(communityId,User.GetUserId(),User.GetUsername());

            return NoContent();
        }

        [HttpPost]
        [ActionName("create")]
        public async Task<ActionResult> Create(AddCommunityDto addCommunityDto)
        {
            var canAdd = await CanAddCommunity(addCommunityDto.Name);
            if (!canAdd) return BadRequest("You cannot create community");

            if (
                await _unitOfWork
                    .CommunityRepository
                    .CreateCommunity(addCommunityDto, User.GetUserId())
            ) return Ok();
            return BadRequest("Creating community failed");
        }

        private async Task<bool> CanAddCommunity(string addedCommunityName)
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

            if (
                await _unitOfWork
                    .CommunityRepository
                    .IsCommunityNameExists(addedCommunityName)
            )
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CanDeleteCommunity(int communityId)
        {
            // var numOfUsersInCommunity =
            //     _unitOfWork
            //         .CommunityRepository
            //         .GetNumOfUsersInCommunity(communityId);

            // if (numOfUsersInCommunity > 5) return false;

            var community =
                _unitOfWork
                    .CommunityRepository
                    .GetCommunitiesFull(User.GetUserId())
                    .SingleOrDefault(x => x.Id == communityId);

            if (community.NumOfMembers > 5) return false;

            if (
                community == null ||
                !community.IsCurrentUserMember ||
                !community.IsCurrentUserCreator
            ) return false;
            return true;
        }
    }
}
