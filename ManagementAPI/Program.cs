using dotenv.net;
using ManagementAPI;
using ManagementAPI.Helpers;
using ManagementAPI.Middleware;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCustomControllers()
    .AddCustomServices()
    .AddCustomDatabase()
    .AddCustomCors()
    .AddSwagger()
    .AddSwaggerExamples()
    .AddJwt();

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
// app.UseHttpsRedirection();
app.MapControllers();

// seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ManagementAPI.Context.DbContext>();
    DatabaseSeeder.Seed(context);
}
app.Run();
