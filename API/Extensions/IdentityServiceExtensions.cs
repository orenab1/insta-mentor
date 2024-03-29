using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection
        AddIdentityServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding
                                        .UTF8
                                        .GetBytes(config["TokenKey"])),
                                   //     .GetBytes("1d20c454-4f6c-45c4-a41a-98acdd245b75")),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };

                    options.Events =
                        new JwtBearerEvents {
                            OnMessageReceived =
                                context =>
                                {
                                    var accessToken =
                                        context.Request.Query["access_token"];

                                    var path = context.HttpContext.Request.Path;
                                    if (
                                        !string.IsNullOrEmpty(accessToken) &&
                                        path.StartsWithSegments("/hubs")
                                    )
                                    {
                                        context.Token = accessToken;
                                    }

                                    return Task.CompletedTask;
                                }
                        };
                });

            return services;
        }
    }
}
