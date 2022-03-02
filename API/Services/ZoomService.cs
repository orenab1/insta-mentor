using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using API.Settings;
using API.SignalR;
using API.Services;
using AutoMapper;
using DAL;
using DAL.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;


namespace API.Services
{
    public class ZoomService : IZoomService
    {
        private readonly ZoomSettings _zoomSettings;

        private static string
            restClientUrl = "https://api.zoom.us/v2/users/me/meetings";

        private static int linkExpirationSeconds = 600;

        public ZoomService( IOptions<ZoomSettings> zoomSettings)
        {
             this._zoomSettings = zoomSettings.Value;
        }

        public async Task<string> GetMeetingUrl(string topic)
        {
            var tokenHandler =
                new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var apiSecret = _zoomSettings.APISecret;
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor =
                new SecurityTokenDescriptor {
                    Issuer = _zoomSettings.APIKey,
                    Expires = now.AddSeconds(linkExpirationSeconds),
                    SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                            SecurityAlgorithms.HmacSha256)
                };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            RestClient client = new RestClient(restClientUrl);

            var request = new RestRequest();

            request.RequestFormat = DataFormat.Json;
            request
                .AddJsonBody(new {
                    topic = topic,
                    type = "1",
                    settings = new { join_before_host = true }
                });
            request
                .AddHeader("authorization",
                String.Format("Bearer {0}", tokenString));
            var restResponse = await client.PostAsync<object>(request);
            var responseAsJObject = JObject.Parse(restResponse.ToString());

            string meetingUrl = responseAsJObject["start_url"].ToString();
            return meetingUrl;
        }
    }
}
