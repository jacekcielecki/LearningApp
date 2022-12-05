using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Services;
using WSBLearn.Application.Settings;
using WSBLearn.Dal.Extensions;
using WSBLearn.WebApi.Extensions;
using WSBLearn.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration;

var jwtSettings = new JwtAuthenticationSettings();
var blobSettings = new AzureBlobStorageSettings();
appConfig.GetSection("Jwt").Bind(jwtSettings);
appConfig.GetSection("BlobStorage").Bind(blobSettings);

AddServicesDependencyInjection();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwagger();
builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.RequireHttpsMetadata = false;
    jwtBearerOptions.SaveToken = true;
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
    };
});
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
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

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

void AddServicesDependencyInjection()
{
    builder.Services.AddTransient<IImageService, ImageService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IUserProgressService, UserProgressService>();
    builder.Services.AddScoped<IQuestionService, QuestionService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAchievementService, AchievementService>();
    builder.Services.AddScoped<ICategoryGroupService, CategoryGroupService>();
}