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
var connectionString = appConfig.GetConnectionString("learningAppDb");

builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApplicationServices(jwtSettings, blobSettings);
builder.Services.AddWebApiServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddMvc();
builder.Services.AddSwagger();
builder.Services.AddAuthentication(jwtSettings);
builder.Services.AddCors(corsOriginName);


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
