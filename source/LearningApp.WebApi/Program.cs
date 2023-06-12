using LearningApp.Application.Extensions;
using LearningApp.Application.Settings;
using LearningApp.Infrastructure.Extensions;
using LearningApp.WebApi.Extensions;
using LearningApp.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration;

var jwtSettings = new JwtAuthenticationSettings();
var blobSettings = new AzureBlobStorageSettings();
appConfig.GetSection("Jwt").Bind(jwtSettings);
appConfig.GetSection("BlobStorage").Bind(blobSettings);

builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddSingleton(jwtSettings);
builder.Services.ConfigureAuthentication(jwtSettings);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: appConfig["Cors:originName"], builder =>
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()
        //.WithOrigins(appConfig["AllowedOrigins"])
    );
});
builder.Services.AddLogging();
builder.Services.AddMvc();
builder.Services.AddApplicationServices(jwtSettings, blobSettings);

var app = builder.Build();

app.UseCors(appConfig["Cors:originName"]);
app.ApplyMigrations();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}
