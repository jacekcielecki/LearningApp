using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.User;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<UserDto> GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(int id)
        {
            _userService.Delete(id);
            return Ok("User successfully deleted");
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var updatedUserDto = _userService.Update(id, updateUserRequest);
            return Ok(updatedUserDto);
        }


        [HttpPatch("updateRole/{id}/{roleId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> UpdateUserRole(int id, int roleId)
        {
            var updatedUserDto = _userService.UpdateUserRole(id, roleId);
            return Ok(updatedUserDto);
        }

        [HttpPatch("updatePassword/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserDto> UpdateUserPassword(int id, [FromBody] UpdateUserPasswordRequest updateUserPasswordRequest)
        {
            _userService.UpdateUserPassword(id, updateUserPasswordRequest);
            return Ok();
        }
    }
}
