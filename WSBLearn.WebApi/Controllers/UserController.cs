using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.User;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _userService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet("SortByExp")]
        public async Task<IActionResult> GetSortByExpAsync()
        {
            var response = await _userService.GetSortByExpAsync();
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserRequest request)
        {
            var response = await _userService.UpdateAsync(id, request);
            return Ok(response);
        }

        [HttpPatch("updateRole/{id}/{roleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoleAsync(int id, int roleId)
        {
            var response = await _userService.UpdateUserRoleAsync(id, roleId);
            return Ok(response);
        }

        [HttpPatch("updatePassword/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateUserPasswordAsync(int id, [FromBody] UpdateUserPasswordRequest request)
        {
            await _userService.UpdateUserPasswordAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
