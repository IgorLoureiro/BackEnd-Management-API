using dotenv.net;
using ManagementAPI;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCustomControllers()
    .AddCustomServices()
    .AddCustomDatabase()
    .AddCustomCors()
    .AddSwagger()
    .AddJwt();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();