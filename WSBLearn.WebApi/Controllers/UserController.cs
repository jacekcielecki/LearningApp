using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.User;
using WSBLearn.Application.Responses;

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
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var dtos = _userService.GetAll();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById(int id)
        {
            var dto = _userService.GetById(id);
            return Ok(dto);
        }

        [HttpGet("SortByExp")]
        public ActionResult<IEnumerable<UserRankingResponse>> GetUsersSortByExp()
        {
            var dtos = _userService.GetSortByExp();
            return Ok(dtos);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(int id)
        {
            _userService.Delete(id);
            return Ok("User successfully deleted");
        }
    }
}
