using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Models.Shared
{
    public class ChangeableFieldData
    {
        public long Id { get; set; } = 0;
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string Label { get; set; } = "";
        public int Type { get; set; }

        public bool CanEdit { get; set; } = false;
       
    }
}
