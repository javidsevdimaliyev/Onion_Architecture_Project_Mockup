using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolutionName.Application.Abstractions.Services;
using SolutionName.Application.Features.Commands.AccountCommands.AssignRoleToUser;
using SolutionName.Application.Features.Commands.AccountCommands.Create;
using SolutionName.Application.Features.Commands.AccountCommands.PasswordReset;
using SolutionName.Application.Features.Queries.AccountQueries.GetAllUsers;
using SolutionName.Application.Features.Queries.AccountQueries.GetRolesToUser;
using SolutionName.Application.Attributes;
using SolutionName.Application.Enums;

namespace SolutionName.WebAPI.Controllers.Auth
{
    public class UserController : Controller
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;
        public UserController(IMediator mediator, IMailService mailService)
        {
            _mediator = mediator;
            _mailService = mailService;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> CreateUser(SignUpRequest request)
        {
            SignUpResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            UpdatePasswordResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, RoleName="Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest getAllUsersQueryRequest)
        {
            GetAllUsersResponse response = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, RoleName="Admin")]
        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserRequest request)
        {
            GetRolesToUserResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, RoleName = "Admin")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserResponse response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }
    }
}
