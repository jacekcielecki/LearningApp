using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;

namespace WSBLearn.WebApi.Controllers
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
        public async Task<IActionResult> QuizCompletedAsync(int categoryId, int level, string quizLevelName, int expGained)
        {
            var userId = HttpContext.GetUserId();
            var response = await _userProgressService.CompleteQuizAsync(userId, categoryId, quizLevelName, expGained);

            return Ok(response);
        }

        [HttpPatch("CompleteAchievement/{userId}/{achievementId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteAchievement(int userId, int achievementId)
        {
            await _userProgressService.CompleteAchievementAsync(userId, achievementId);
            return Ok();
        }
    }
}
