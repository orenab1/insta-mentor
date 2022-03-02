using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL;
using DAL.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using System.Security.Claims;
using System.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using DAL.Interfaces;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMessagesService _messagesService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(
            ITokenService tokenService, 
            IMessagesService messagesService,
            IUnitOfWork unitOfWork)
        {
            this._tokenService = tokenService;
            this._messagesService = messagesService;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Email))
            {
                return BadRequest("Email already exists. If you forgot your password, please consider navigating to \"Sign In\", and clicking \"Forgot Password\"");
            }

            using var hmac = new HMACSHA512();

            Random rnd=new Random();


            var user = new AppUser
            {
                PasswordHash = getComutedHash(registerDto.Password,hmac.Key),
                PasswordSalt = hmac.Key,
                Created=DateTime.Now
            };

            if (user.EmailPrefrence==null)
            {
                user.EmailPrefrence=new EmailPrefrence();
            }

            await this._unitOfWork.AccountRepository.CreateUserAsync(user);


            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Id = user.Id
            };
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<UserDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto){
            if (await UserExists(forgotPasswordDto.Email))
            {
                throw new NotImplementedException();
            }else{
                return BadRequest("Email address not found. Please make sure you typed it correctly, or register");
            }
        }

        [HttpPost]
        [ActionName("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await this._unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.Email);
            
            if (user == null)
            {
                return Unauthorized("Email address not found. Please make sure you typed it correctly, or register");
            }

           var computedHash=getComutedHash(loginDto.Password,user.PasswordSalt);

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Your email and password do not match. Please try again or click \"Forgot Password\"");
                }
            }

            await _messagesService.NotifyAskersOffererLoggedInAsync(user.Id);

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photo == null ? "" : user.Photo.Url,
                Id = user.Id
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await this._unitOfWork.AccountRepository.IsUserExistsAsync(email);
           // return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        private byte[] getComutedHash(string text,byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);

            return hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}