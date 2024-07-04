using System.ComponentModel.DataAnnotations;
using MediatR;


namespace SolutionName.Application.Features.Commands.Account.Login
{
    public class LoginByPasswordRequest : IRequest<LoginByPasswordResponse>
    {
 
        /// <summary>
        ///     Email
        /// </summary>
        [Required]
        public string UsernameOrEmail { get; set; }

        /// <summary>
        ///     password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
