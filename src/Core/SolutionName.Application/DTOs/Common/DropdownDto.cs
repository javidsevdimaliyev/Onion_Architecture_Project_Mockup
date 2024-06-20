using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolutionName.Application.DTOs.Common
{
    public sealed class DropDownDto : BaseDto<int>
    {
        public string ValueHash { get; set; }

        [JsonIgnore]
        public int Value { get { return Decrypt<int>(ValueHash); } set { ValueHash = Encrypt(value); } }

        public string DisplayText { get; set; }
        public bool Select { get; set; }


    }

}
