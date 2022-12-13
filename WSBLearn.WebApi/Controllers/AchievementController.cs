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
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _achievementService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAchievementRequest request)
        {
            var response = await _achievementService.CreateAsync(request);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAchievementRequest request)
        {
            var response = await _achievementService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _achievementService.DeleteAsync(id);
            return Ok();
        }
    }
}
