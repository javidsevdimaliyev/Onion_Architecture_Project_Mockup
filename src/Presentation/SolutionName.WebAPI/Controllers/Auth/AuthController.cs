﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolutionName.Application.Features.Commands.AccountCommands.Login;
using SolutionName.Application.Features.Commands.AccountCommands.RefreshToken;

namespace SolutionName.WebAPI.Controllers.Auth
{
    [AllowAnonymous]
    [Route("api/v{v:apiVersion}/auth")]
    public class AuthController : BaseController
    {

        readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginByPasswordRequest loginUserCommandRequest)
        {
            LoginByPasswordResponse response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenRequest refreshTokenLoginCommandRequest)
        {
            LoginByPasswordResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }

      
    }
}
