using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests;
using WSBLearn.Domain.Entities;

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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            string token = _userService.Login(loginDto);
            return Ok(token);
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] CreateUserRequest createUserRequest)
        {
            _userService.Register(createUserRequest);
            return Ok();
        }
    }
}
