using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using DAL;
using DAL.DTOs;
using DAL.DTOs.Summary;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IPhotoService _photoService;

        public UsersController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPhotoService photoService
        )
        {
            this._mapper = mapper;
            this._photoService = photoService;
            this._unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("{username}")]
        [ActionName("get-user")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _unitOfWork.UserRepository.GetMemberAsync(username);
        }

        [HttpGet("{username}")]
        [ActionName("get-user-summary")]
        public async Task<ActionResult<UserSummaryDto>>
        GetUserSummary(string username)
        {
            return await _unitOfWork
                .UserRepository
                .GetUserSummaryDtoAsync(username);
        }

        [HttpPut("update-user")]
        public async Task<ActionResult>
        UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser user =
                await _unitOfWork.UserRepository.GetUserAsync(username);

            TagDto[] newTags = memberUpdateDto.Tags.ToArray();
            CommunityDto[] newCommunities =
                memberUpdateDto.Communities.ToArray();

            memberUpdateDto.Tags = null;
            memberUpdateDto.Communities = null;

            _mapper.Map (memberUpdateDto, user);

            await _unitOfWork.Complete();

            await _unitOfWork.TagRepository.UpdateTagsForUser(newTags, user.Id);

            _unitOfWork.UserRepository.Update (user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPut("{isOnline}", Name = "change-current-user-online-status")]
        public async Task<ActionResult>
        ChangeCurrentUserOnlineStatus(bool isOnline)
        {
            var username = User.GetUsername();
            _unitOfWork
                .UserRepository
                .ChangeCurrentUserOnlineStatus(User.GetUsername(), isOnline);
            await _unitOfWork.Complete();
            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user =
                await _unitOfWork
                    .UserRepository
                    .GetUserAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo =
                new Photo {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

            user.Photo = photo;

            if (await _unitOfWork.Complete())
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
            var user =
                await _unitOfWork
                    .UserRepository
                    .GetUserAsync(User.GetUsername());

            var photo = user.Photo;

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result =
                    await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            user.Photo = null;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}
