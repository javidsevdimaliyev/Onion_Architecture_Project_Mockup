using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Features.Queries.AccountQueries.GetAllUsers
{
    public class GetAllUsersResponse
    {
        public object Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}
