using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProgressController : ControllerBase
    {
        private readonly IUserProgressService _userProgressService;
        private readonly ClaimsPrincipal _userContext;


        public UserProgressController(IUserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
            _userContext = HttpContext.GetUserContext();
        }

        [HttpPatch("{categoryId}/{level}/{quizLevelName}/{expGained}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteQuizAsync(int categoryId, int level, string quizLevelName, int expGained)
        {
            var response = await _userProgressService.CompleteQuizAsync(_userContext, categoryId, quizLevelName, expGained);
            return Ok(response);
        }

        [HttpPatch("CompleteAchievement/{userId}/{achievementId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteAchievementAsync(int userId, int achievementId)
        {
            await _userProgressService.CompleteAchievementAsync(userId, achievementId);
            return Ok();
        }
    }
}
