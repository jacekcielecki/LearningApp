using LearningApp.Application.Extensions;
using LearningApp.Application.Settings;
using LearningApp.Infrastructure.Extensions;
using LearningApp.WebApi.Extensions;
using LearningApp.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtAuthenticationSettings>();
var blobSettings = builder.Configuration.GetSection("BlobStorage").Get<AzureBlobStorageSettings>();
var smtpSettings = builder.Configuration.GetSection("Smtp").Get<SmtpSettings>();
var connectionString = builder.Configuration.GetConnectionString("learningAppDb");
var corsOriginName = builder.Configuration["Cors:originName"];

builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApplicationServices(jwtSettings, blobSettings, smtpSettings);
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
