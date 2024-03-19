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

        [HttpPatch("CompleteQuiz/Category/{categoryId}/Level/{quizLevelName}/ExpGained/{expGained}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteQuizAsync(int categoryId, string quizLevelName, int expGained)
        {
            var response = await _userProgressService.CompleteQuizAsync(categoryId, quizLevelName, expGained);
            return Ok(response);
        }

        [HttpPatch("CompleteAchievement/Achievement/{achievementId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteAchievementAsync(int achievementId)
        {
            await _userProgressService.CompleteAchievementAsync(achievementId);
            return Ok();
        }
    }
}
