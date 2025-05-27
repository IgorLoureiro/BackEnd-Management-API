using dotenv.net;
using ManagementAPI;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCustomControllers()
    .AddCustomServices()
    .AddCustomDatabase()
    .AddCustomCors()
    .AddSwagger();

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