using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
{
#pragma warning disable CS8632
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
        [TypeFilter<GetUserFilter>]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CompleteAchievementAsync(int achievementId, [FromServices] UserDto? user)
        {
            await _userProgressService.CompleteAchievementAsync(achievementId, user);
            return Ok();
        }
    }
#pragma warning restore CS8632
}
