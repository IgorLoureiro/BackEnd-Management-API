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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ManagementAPI.Helpers;

namespace ManagementAPI.Extensions
{
    public static class ServiceExtensions
    {
        private const string SecuritySchemeName = "Bearer";

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
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new Exception("DB_CONNECTION_STRING não configurada.");

            var dbInitializer = new DatabaseInitializer();
            dbInitializer.EnsureDatabaseExists(connectionString);

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

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(SecuritySchemeName, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = string.Join(SecuritySchemeName, " {token}") 
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = SecuritySchemeName
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }


        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER não configurado."),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE não configurado."),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ??  throw new Exception("JWT_SECRET não configurado.")))
                };
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
