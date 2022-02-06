using API.Helpers;
using API.Interfaces;
using API.Services;
using API.Settings;
using API.SignalR;
using AutoMapper;
using DAL;
using DAL.Data;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection
        AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddSingleton<PresenceTracker>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services
                .Configure<CloudinarySettings>(config
                    .GetSection("CloudinarySettings"));
            services
                .Configure<MailSettings>(config
                    .GetSection("MailSettings"));
             services
                .Configure<ZoomSettings>(config
                    .GetSection("ZoomSettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof (AutoMapperProfiles).Assembly);

            services
                .AddDbContext<DataContext>(options =>
                {
                    options
                        .UseSqlite(config
                            .GetConnectionString("DefaultConnection"));
                });

            return services;
        }
    }
}
