using SolutionName.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Domain.Entities
{
    public class CustomerEntity : AuditableEntity<int>
    {
        public string Name { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
