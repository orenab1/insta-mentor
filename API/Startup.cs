using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using API.Services;
using API.Settings;
using API.SignalR;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;


namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        private readonly string
            MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services
            //     .Configure<CookiePolicyOptions>(options =>
            //     {
            //         // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //         // options.CheckConsentNeeded = context => true;
            //         options.MinimumSameSitePolicy = SameSiteMode.None;
                    
            //         // options.Cookie.SameSite = SameSiteMode.None; 
            //     });

            services.AddApplicationServices (_config);

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services
                .AddSwaggerGen(c =>
                {
                    c
                        .SwaggerDoc("v1",
                        new OpenApiInfo { Title = "API", Version = "v1" });
                });

            services
                .AddCors(options =>
                {
                    options
                        .AddPolicy(name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder
                                .WithOrigins("https://vidcallme.azurewebsites.net","https://localhost:4200", "https://vidcallme-app.azurewebsites.net","https://vidcallme.azurewebsites.net/hubs/", "http://vidcallme.com","http://vidcallme.com/hubs/", "http://www.vidcallme.com", "http://www.vidcallme.com/hubs/")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        });
                });

            services.AddIdentityServices (_config);

            services.AddSignalR();

            services.AddSingleton(Log.Logger);

            services
                .AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId =
                        "490846212400326";
                    facebookOptions.AppSecret =
                        "8bb6f6b0a64fcf5d88e6f48bfd10e026";
                })
                .AddGoogle(options =>
                {
                    //     IConfigurationSection googleAuthNSection =
                    //         Configuration.GetSection("Authentication:Google");
                    options.ClientId =
                        "155014635586-ir4obp64jsr2do7e2cdnh0msauq11lbo.apps.googleusercontent.com";
                    options.ClientSecret =
                        "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";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

        //     Log.Logger = new LoggerConfiguration()
        // .ReadFrom.Configuration(_config)
        // .CreateLogger();

            // Log.Logger=new LoggerConfiguration()
            //     .ReadFrom.Configuration(Configuration)
            //     .Enrich.FromLogContext()
            //     .CreateLogger();

            // loggerFactory.AddSerilog();

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app
                    .UseSwaggerUI(c =>
                        c
                            .SwaggerEndpoint("/swagger/v1/swagger.json",
                            "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors (MyAllowSpecificOrigins);

            // app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200/"));
            //  app.UseCookiePolicy();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseAuthentication();
            app.UseAuthorization();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<PresenceHub>("hubs/presence");
                });

        }

        private void CheckSameSite(
            HttpContext httpContext,
            CookieOptions options
        )
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent =
                    httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }

        public static bool DisallowsSameSiteNone(string userAgent)
        {
            // Check if a null or empty string has been passed in, since this
            // will cause further interrogation of the useragent to fail.
            if (String.IsNullOrWhiteSpace(userAgent)) return false;

            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking
            // stack.
            if (
                userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12")
            )
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (
                userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") &&
                userAgent.Contains("Safari")
            )
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None,
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6")
            )
            {
                return true;
            }

            return false;
        }
    }
}
