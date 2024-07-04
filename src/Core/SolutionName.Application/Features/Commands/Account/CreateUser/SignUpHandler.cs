using MediatR;
using SolutionName.Application.Abstractions.Services.Authentication;

namespace SolutionName.Application.Features.Commands.Account.Create
{
    internal class SignUpHandler : IRequestHandler<SignUpRequest, SignUpResponse>
    {
        readonly IUserService _userService;
        public SignUpHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<SignUpResponse> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            var res = await _userService.AddAsync(new()
            {
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                Username = request.Username,
            });

            return res;
        }
    }
}
