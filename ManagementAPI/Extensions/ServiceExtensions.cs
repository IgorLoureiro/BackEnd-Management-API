using ManagementAPI.Repository;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbContext = ManagementAPI.Context.DbContext;

namespace ManagementAPI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.Filters.Add<ExceptionFilter>();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDefaultUserRepository, DefaultUserRepository>();
            return services;
        }

        public static IServiceCollection AddCustomDatabase(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContext<DbContext>(options =>
            {
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            });

            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
