using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;

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
        [AllowAnonymous]
        public ActionResult DeleteUser(int id)
        {
            _userService.Delete(id);
            return Ok("User successfully deleted");
        }

        [HttpPatch("{id}")]
        [AllowAnonymous]
        public ActionResult<UserDto> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var updatedUserDto = _userService.Update(id, updateUserRequest);
            return Ok(updatedUserDto);
        }
    }
}
