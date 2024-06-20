namespace SolutionName.Application.DTOs.Account.Authentication.User
{
    public class UserCreateRequestDto : BaseDto<long>, IMapTo<UserEntity>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
