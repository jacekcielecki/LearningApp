using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Services;
using WSBLearn.Dal.Extensions;
using WSBLearn.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var appConfig = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = appConfig["Jwt:Issuer"],
            ValidAudience = appConfig["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig["Jwt:Key"]))
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
builder.Services.AddApplicationServices();

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
