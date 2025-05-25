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

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
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



builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); 
    options.Filters.Add<ExceptionFilter>();   
});

/* Setup to allowed custom filters */
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
   options.SuppressModelStateInvalidFilter = true; 
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
});

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseMySql(
        dbConfig.ConnectionString,
        ServerVersion.AutoDetect(dbConfig.ConnectionString)
    );
});

// Registra como singleton pra injetar onde quiser depois
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton(dbConfig);

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IDefaultUserRepository, DefaultUserRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();