using LearningApp.Application.Extensions;
using LearningApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProgressController : ControllerBase
    {
        private readonly IUserProgressService _userProgressService;

        public UserProgressController(IUserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }

        [HttpPatch("{categoryId}/{level}/{quizLevelName}/{expGained}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteQuizAsync(int categoryId, int level, string quizLevelName, int expGained)
        {
            var userContext = HttpContext.GetUserContext();
            var response = await _userProgressService.CompleteQuizAsync(userContext, categoryId, quizLevelName, expGained);
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
