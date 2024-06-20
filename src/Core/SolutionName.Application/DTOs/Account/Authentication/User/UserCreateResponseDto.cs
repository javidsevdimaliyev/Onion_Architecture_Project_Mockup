namespace SolutionName.Application.DTOs.Account.Authentication.User
{
    public class UserCreateResponseDto : BaseDto<long>
    {
        public string Email { get; set; }
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
