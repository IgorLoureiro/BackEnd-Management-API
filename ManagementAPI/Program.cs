using Microsoft.EntityFrameworkCore;
using DbContext = ManagementAPI.Context.DbContext;
using dotenv.net;
using ManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ManagementAPI.Repository;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

/* Build controllers and add custom filters */
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); 
    options.Filters.Add<ExceptionFilter>();   
});

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDefaultUserRepository, DefaultUserRepository>();

/* Setup to allowed custom filters */
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
   options.SuppressModelStateInvalidFilter = true; 
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.MapControllers();
app.Run();