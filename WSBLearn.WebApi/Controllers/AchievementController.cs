using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.Achievement;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _achievementService.GetAll();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateAchievementRequest request)
        {
            var response = _achievementService.Create(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] UpdateAchievementRequest request)
        {
            var response = _achievementService.Update(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _achievementService.Delete(id);
            return Ok();
        }
    }
}
