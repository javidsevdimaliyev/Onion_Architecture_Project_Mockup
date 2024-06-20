namespace SolutionName.Application.Constants
{
    public struct ValidationRegExpressionConsts
    {
        public const string Email = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string Password = @"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{7,}";

        public const string Min6Character = @"^.{6,}$";
        public const string Min3Character = @"^.{3,}$";
        public const string Only7Characters = @"^[a-zA-Z0-9]{7,7}$";
        public const string Only6Digit = @"^[0-9]{1,6}$";
        public const string Min10Digit = @"^.[0-9]{9,}$";
        public const string Max20Digit = @"^[0-9]{1,20}$";
        public const string MustContainCapitalCase = @"[A-Z]+";
        public const string MustContainLowerCase = @"[a-z]+";
        public const string MustContainDigit = @"(\d)+";
        public const string MustContainAlphanumeric = @"^[a-zA-Z0-9]+$";
        public const string MustContainOnlyAlphabetic = @"^[a-zA-Z ]*$";
        public const string MustContainOnlyNumeric = @"^[0-9]+$";
    }
}
