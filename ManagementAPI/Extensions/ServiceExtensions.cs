using ManagementAPI.Repository;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbContext = ManagementAPI.Context.DbContext;
using ManagementAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.Interfaces;
using ManagementAPI.Interceptors;
using System.Text.Json.Serialization;

namespace ManagementAPI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.Filters.Add<NotFoundFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IDefaultUserRepository, DefaultUserRepository>();
            services.AddScoped<IMailerService, MailerService>();

            return services;
        }

        public static IServiceCollection AddCustomDatabase(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

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
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.ExampleFilters();
            });

            return services;
        }

        public static IServiceCollection AddSwaggerExamples(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<LoginOtpResponseDtoExample>();
            services.AddSwaggerExamplesFromAssemblyOf<LoginOtpRequestDtoExample>();
            services.AddSwaggerExamplesFromAssemblyOf<SendOtpRequestDtoExample>();

            return services;
        }
    }
}
