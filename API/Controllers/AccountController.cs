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
using API.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Serilog;
using Serilog.AspNetCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMessagesService _messagesService;
        private readonly IUnitOfWork _unitOfWork;
        readonly ILogger<HomeController> _logger;

        public AccountController(
            ITokenService tokenService, 
            IMessagesService messagesService,
            IUnitOfWork unitOfWork,
            ILogger<HomeController> logger)
        {
            this._tokenService = tokenService;
            this._messagesService = messagesService;
            this._unitOfWork = unitOfWork;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
             _logger.LogDebug("Hello, world!");
             Log.CloseAndFlush();
        }

        [HttpPost]
        [ActionName("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            

            if (await UserExists(registerDto.Email))
            {
                return BadRequest("Email already exists. If you forgot your password, please consider navigating to \"Sign In\", and clicking \"Forgot Password\"");
            }

            using var hmac = new HMACSHA512();

            Random rnd=new Random();

            string verificationCode=Guid.NewGuid().ToString();

            var user = new AppUser
            {
                PasswordHash = getComputedHash(registerDto.Password,hmac.Key),
                PasswordSalt = hmac.Key,
                Created=DateTime.UtcNow,
                VerificationCode=verificationCode,
                Email=registerDto.Email,
                Password=registerDto.Password
            };

            if (user.EmailPrefrence==null)
            {
                user.EmailPrefrence=new EmailPrefrence();
            }

            await this._unitOfWork.AccountRepository.CreateUserAsync(user);


           _messagesService.SendVerificationEmail(registerDto.Email, verificationCode);

            return new UserDto
            {
                Username = user.UserName,
              //  Token = _tokenService.CreateToken(user),
                Id = user.Id
            };
        }

        [HttpPost]
        [ActionName("forgot-password")]
        public async Task<ActionResult<UserDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto){

            AppUser user=await _unitOfWork.UserRepository.GetUserByEmailAsync(forgotPasswordDto.Email);

           if (user==null){
                return BadRequest("Email address not found. Please make sure you typed it correctly, or register");
           }

            if (user.IsVerified){
                _messagesService.SendPasswordEmail(forgotPasswordDto.Email, user.Password);
                return BadRequest("Your password has been sent to the email address you provided");
            }else {
                _messagesService.SendPasswordAndVerificationEmail(forgotPasswordDto.Email, user.VerificationCode,user.Password);
                return BadRequest("Your account has not been verified yet. Your password, and a verification link has been sent to the email address you provided");
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

           var computedHash=getComputedHash(loginDto.Password,user.PasswordSalt);

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Your email and password do not match. Please try again or click \"Forgot Password\"");
                }
            }

            if (!user.IsVerified){
                if (user.VerificationCode==loginDto.VerificationCode)
                {
                    await _unitOfWork.UserRepository.MarkUserAsVerified(user.Id);                    
                }
                else{
                    return Unauthorized("Something went wrong. Please email us at oren@vidcallme.com");
                }
            }


            await _messagesService.NotifyAskersOffererLoggedInAsync(user.Id);
            _messagesService.SendMeUserSignedInEmail(user.UserName);

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

        private byte[] getComputedHash(string text,byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);

            return hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}