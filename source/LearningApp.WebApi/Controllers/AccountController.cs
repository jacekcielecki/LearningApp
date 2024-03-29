﻿using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userService.LoginAsync(loginDto);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest)
        {
            var userEmail = await _userService.RegisterAsync(createUserRequest);
            await _emailService.SendAccountVerificationEmail(userEmail);
            return Ok();
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyAccount(string verificationToken)
        {
            await _userService.VerifyAccount(verificationToken);
            return Ok();
        }

        [HttpPatch("sendVerificationEmail")]
        public async Task<IActionResult> SendAccountVerificationEmail(string userEmail)
        {
             await _emailService.SendAccountVerificationEmail(userEmail);
            return Ok();
        }

        [HttpPatch("forget-password")]
        public async Task<IActionResult> GetPasswordResetToken(string userEmail)
        {
            var token = await _userService.GetPasswordResetToken(userEmail);
            return Ok(token);
        }

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _userService.ResetPassword(request);
            return Ok();
        }
    }
}
