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

        [HttpGet("{id}")]
        [ActionName("get-user-summary-by-id")]
        public async Task<ActionResult<UserSummaryDto>>
        GetUserSummaryById(int id)
        {
            return await _unitOfWork.UserRepository.GetUserSummaryDtoById(id);
        }

        
        [HttpGet("{username}")]
        [ActionName("get-user")]
        public async Task<ActionResult<UserFullDto>> GetUser(string username)
        {
            return await _unitOfWork
                .UserRepository
                .GetUserAsync(User.GetUserId());
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

        [HttpPut]
        [ActionName("update-user")]
        public async Task<ActionResult>
        UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            if(await _unitOfWork
                    .UserRepository.DoesUsernameExistForDifferentUser(User.GetUserId(),memberUpdateDto.Username)){
                return BadRequest("Username already exists, please try a different username");
            }

            var k=User.GetUsername();

            if ((User.GetUsername() != memberUpdateDto.Username) && memberUpdateDto.Username.StartsWith("Guest")){
                return BadRequest("Please don't change your username to a username that starts with the word \"Guest\"");
            }


            AppUser user =
                await _unitOfWork
                    .UserRepository
                    .GetUserByIdAsync(User.GetUserId());

            _mapper.Map (memberUpdateDto, user);

            user.Communities = null;

            await _unitOfWork.Complete();

            await _unitOfWork
                .UserRepository
                .UpdateCommunitiesForUser(memberUpdateDto.Communities,
                User.GetUserId());

            TagDto[] newTags = memberUpdateDto.Tags.ToArray();

            memberUpdateDto.Tags = null;

            await _unitOfWork
                .TagRepository
                .UpdateTagsForUser(newTags, User.GetUserId());

           

            return NoContent();

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

        [HttpPost]
        [ActionName("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user =
                await _unitOfWork
                    .UserRepository
                    .GetUserByIdAsync(User.GetUserId());

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
                return _mapper.Map<PhotoDto>(photo);

                // return CreatedAtRoute("GetUser",
                // new { username = user.UserName },
                // _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem addding photo");
        }

        [HttpDelete]
        [ActionName("delete-photo")]
        public async Task<ActionResult> DeletePhoto()
        {
            var user =
                await _unitOfWork
                    .UserRepository
                    .GetUserByIdAsync(User.GetUserId());

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
