using Microsoft.EntityFrameworkCore;
using DbContext = ManagementAPI.Context.DbContext;
using dotenv.net;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ManagementAPI.Settings;
using System.Text.Json.Serialization;

using Swashbuckle.AspNetCore.Filters;
using ManagementAPI.SwaggerExamples;
using ManagementAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var jwtConfig = JwtConfig.FromEnvironment();
var dbConfig = DbConfig.FromEnvironment();

// Configurar autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // deixar true em produção
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
    };
});

builder.Services.AddAuthorization();

/* Build controllers and add custom filters */
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IDefaultUserRepository, DefaultUserRepository>();
builder.Services.AddScoped<IMailerService, MailerService>();

/* Setup to allowed custom filters */
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    options.EnableAnnotations();
    options.ExampleFilters();
});

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// Registra como singleton pra injetar onde quiser depois
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton(dbConfig);


/* exemplos do DTO */
builder.Services.AddSwaggerExamplesFromAssemblyOf<MailerRequestDtoExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginOtpResponseDtoExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<LoginOtpRequestDtoExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<SendOtpRequestDtoExample>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
// app.UseHttpsRedirection();
app.MapControllers();
app.Run();