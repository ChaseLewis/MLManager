using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MLManager.Services;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<JwtResponse>> Refresh()
        {
            var jwtResponse = await _authenticationService.Refresh(User.GetUserId(),Guid.Empty,null);

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
    }
}
