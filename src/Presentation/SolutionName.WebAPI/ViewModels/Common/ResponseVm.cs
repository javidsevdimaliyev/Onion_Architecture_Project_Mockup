using SolutionName.Application.DTOs.Common;

namespace SolutionName.WebAPI.ViewModels.Common
{
    public class ResponseVm
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class ResponseVm<DTOType> : ResponseVm where DTOType : BaseDto
    {
        public DTOType Data { get; set; }
       
    }
}
