using MediatR;

namespace SolutionName.Application.Features.Commands.AccountCommands.PasswordReset
{
    public class UpdatePasswordRequest : IRequest<UpdatePasswordResponse>
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
