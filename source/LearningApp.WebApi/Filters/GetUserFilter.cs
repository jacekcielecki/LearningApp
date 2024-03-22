using LearningApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningApp.WebApi.Filters;

public class GetUserFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserFilter(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var jtiClaim = _httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(c => c.Type == "jti");
        var userId = Convert.ToInt32(jtiClaim?.Value);

        var user = await _userService.GetByIdAsync(userId);

        ArgumentNullException.ThrowIfNull(user, "User with given id not found");

        context.ActionArguments["user"] = user;
        await next();
    }
}
