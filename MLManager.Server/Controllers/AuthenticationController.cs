using System;
using System.Collections.Generic;
using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MLManager.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace MLManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAuthenticationService authenticationService
            )
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<JwtResponse>> Refresh(RefreshRequest request)
        {
            var jwtResponse = await _authenticationService.Refresh(User.GetUserId(),request.RefreshToken,request.DeviceId);

            if(jwtResponse == null)
                return Unauthorized("Refresh token does not exist or does not match the expected user information.");

            return Ok(jwtResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult<JwtResponse>> Authenticate(AuthenticationRequest request)
        {
            var jwtResponse = await _authenticationService.Authenticate(request.Username,request.Password,request.DeviceId);

            if(jwtResponse == null)
                return Unauthorized("Username and/or password does not match a current user.");

            return Ok(jwtResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<ActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
                User user = await _authenticationService.CreateUser(request);

                return Ok(new 
                {
                    user.Username,
                    user.FirstName,
                    user.LastName
                });
            }
            catch(ValidationException ex)
            {
                return BadRequest(new { Message = ex.ValidationResult.ErrorMessage });
            }
            //We need a way to return an exception based on if the username or email would violate stuff
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("username")] //We should make sure this only works on our domain
        public async Task<ActionResult<bool>> UsernameExists(string username)
        {
            if(string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is not a valid value.");

            return await _authenticationService.DoesUsernameExist(username);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("email")] //We should make sure this only works on our domain
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is not a valid value");
                
            return await _authenticationService.DoesEmailExist(email);
        }
    }
}
