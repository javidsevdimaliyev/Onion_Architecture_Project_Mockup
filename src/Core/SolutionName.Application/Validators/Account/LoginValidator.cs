using FluentValidation;
using SolutionName.Application.Features.Commands.AccountCommands.Login;

namespace SolutionName.Application.Validators.Account
{
    public class LoginValidator : AbstractValidator<LoginByPasswordRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UsernameOrEmail).NotNull().NotEmpty().WithMessage("Email or Username can't be empty");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password can not be empty");
        }
    }
}
