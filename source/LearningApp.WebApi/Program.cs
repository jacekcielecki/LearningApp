using LearningApp.Application.Extensions;
using LearningApp.Application.Settings;
using LearningApp.Infrastructure.Extensions;
using LearningApp.WebApi.Extensions;
using LearningApp.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration;
var jwtSettings = appConfig.GetSection("Jwt").Get<JwtAuthenticationSettings>()!;
var blobSettings = appConfig.GetSection("BlobStorage").Get<AzureBlobStorageSettings>()!;
var corsOriginName = appConfig["Cors:originName"]!;

builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.ConfigureAuthentication(jwtSettings);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOriginName, builder =>
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

app.UseCors(corsOriginName);
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
