using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.Interfaces;
using API.Services;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using AutoMapper;
using API.Helpers;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IPhotoService,PhotoService>();
            services.AddScoped<IQuestionRepository,QuestionRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(options =>{
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }); 

            return services;
        }
    }
}