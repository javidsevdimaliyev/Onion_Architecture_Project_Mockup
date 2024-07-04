using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Features.Commands.AccountCommands.AssignRoleToUser
{
    public class AssignRoleToUserRequest : IRequest<AssignRoleToUserResponse>
    {
        public string Userid { get; set; }
        public string[] Roles { get; set; }       
    }
}
