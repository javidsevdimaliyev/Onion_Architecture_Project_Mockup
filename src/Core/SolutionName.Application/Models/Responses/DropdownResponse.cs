using SolutionName.Application.DTOs.Common;

namespace SolutionName.Application.Models.Responses
{
    public class DropdownResponse
    {
        public int Count { get; set; }
        public ICollection<DropDownDto> Data { get; set; }
    }
}
