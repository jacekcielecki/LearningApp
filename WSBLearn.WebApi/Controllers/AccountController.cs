using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Requests.User;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
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
