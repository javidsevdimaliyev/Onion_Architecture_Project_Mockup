using System.Reflection;

namespace SolutionName.Application.Shared.Constants;

public struct CustomClaimTypeConsts
{
    public const string UserId = "UserId";
    public const string PersonId = "PersonId";
    public const string WorkplaceId = "WorkplaceId";
    public const string Email = "Email";
    public const string PersonType = "PersonType";
    public const string NickName = "NickName";
    public const string FullName = "FullName";
    public const string FirstName = "FirstName";
    public const string LastName = "LastName";
    public const string BirthDate = "BirthDate";
    public const string SchemaIds = "SchemaIds";
    public const string Roles = "Roles";
    public const string Claims = "Claims";
    public const string Administrator = "Admin";
    public const string AuthenticationType = "AuthenticationType";
    public const string TwoFactorEnabled = "TwoFactorEnabled";
    public const string AsanSignUserId = "AsanSignUserId";
    public const string AsanPhoneNumber = "AsanPhoneNumber";
    public const string AsanSignCertificateKey = "AsanSignCertificateKey";
    public const string AsanSignCertificateValue = "AsanSignCertificateValue";
    public const string Tin = "Tin";
    public const string Positions = nameof(Positions);
    public const string UserOrganizationId = nameof(UserOrganizationId);
    public const string UserOrganizationName = nameof(UserOrganizationName);

    //public const string UserId = "http://projectdomain.com/UserId";
    public const string UserTypeId = "http://projectdomain.com/UserTypeId";
    public const string Fin = "http://projectdomain.com/Fin";
    //public const string FullName = "http://projectdomain.com/FullName";
    public const string RemoteIpAddress = "http://projectdomain.com/RemoteIpAddress";
    public const string FunctionalRoleClaim = "http://projectdomain.com/FunctionalRoleClaim";


    private static List<FieldInfo> ClaimsList()
    {
        var fields = typeof(CustomClaimTypeConsts)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        return fields.ToList();
    }

    public static List<string> AllClaimTypes()
    {
        return ClaimsList().Select(x => x.GetValue(null).ToString()).ToList();
    }
}