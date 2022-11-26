using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Extensions;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Responses;

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
        public ActionResult<QuizCompletedResponse> QuizCompleted(int categoryId, int level, string quizLevelName, int expGained)
        {
            var userId = HttpContext.GetUserId();
            var quizCompletedResponse = _userProgressService.CompleteQuiz(userId, categoryId, quizLevelName, expGained);

            return Ok(quizCompletedResponse);
        }

        [HttpPatch("CompleteAchievement/{userId}/{achievementId}")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult CompleteAchievement(int userId, int achievementId)
        {
            _userProgressService.CompleteAchievement(userId, achievementId);
            return Ok();
        }
    }
}
