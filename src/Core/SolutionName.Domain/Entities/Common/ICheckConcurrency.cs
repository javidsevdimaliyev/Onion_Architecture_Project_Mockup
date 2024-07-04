using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Domain.Entities.Common
{
    public interface ICheckConcurrency
    {
        [Timestamp]
        public Guid RowVersion { get; set; }
        //public byte[] RowVersion { get; set; }
    }
}
