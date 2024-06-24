using Microsoft.AspNetCore.Http;
using SolutionName.Application.Constants;
using System.Security.Claims;

namespace SolutionName.Application.Utilities.Helpers;

public static class IdentityHelper
{
    public static long UserId => GetUserId();
    public static string? FunctionalRoleClaimValue => GetFunctionalRoleClaimValue();
    public static string? UserName => GetUserName();
    public static ClaimsIdentity UserIdentity => GetUserIdentity();
    public static IEnumerable<Claim> UserClaims => GetUserClaims();
    public static bool UserIsAuthenticated => GetUserIsAuthenticated();
    public static string MainOrganCodeOneMillion => "1000000";
    public static int AuthenticationType
    {
        get
        {
            int.TryParse(GetUserClaimValue(CustomClaimTypeConsts.AuthenticationType), out int authenticationType);

            return authenticationType;
        }
    }

    private static ClaimsIdentity GetUserIdentity()
    {
        return (ClaimsIdentity)new HttpContextAccessor().HttpContext?.User?.Identity;
    }

    public static int GetId()
    {
        var userId = GetUserIdentity().FindFirst(CustomClaimTypeConsts.UserId)?.Value;
        return !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0;
    }

    public static int GetUserId()
    {
        var userId = GetUserIdentity()?.FindFirst(CustomClaimTypeConsts.UserId)?.Value;
        //var result = Convert.ToInt32(GetUserIdentity().FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0;
    }

    private static string? GetFunctionalRoleClaimValue()
    {
        var value = GetUserIdentity()?.FindFirst(CustomClaimTypeConsts.FunctionalRoleClaim)?.Value;

        if (!string.IsNullOrEmpty(value) && value.Contains('*')) value = value.Replace("*", "");
        return value;
    }

    public static string? GetUserFin()
    {
        var fin = GetUserIdentity().FindFirst(CustomClaimTypeConsts.Fin)?.Value;
        return !string.IsNullOrEmpty(fin) ? fin : null;
    }

    private static string? GetUserName()
    {
        var userNameClaim = GetUserClaim(ClaimTypes.Name);
        return userNameClaim?.Value;
    }

   
    private static IEnumerable<Claim> GetUserClaims()
    {
        var identity = GetUserIdentity();
        return identity.Claims;
    }

    private static Claim GetUserClaim(string claimType)
    {
        var identity = GetUserIdentity();
        return identity.Claims.FirstOrDefault(x => x.Type == claimType);
    }


    //public static UserManager<T> GetUserManager<T>() where T : class
    //{
    //    return new HttpContextAccessor().HttpContext.RequestServices.GetService<UserManager<T>>();
    //}

    public static string GetUserClaimValue(string claimName)
    {
        var identity = GetUserIdentity().Claims.FirstOrDefault(x => x.Type == claimName);
        if (identity != null) return identity?.Value;
        return "";
    }

    public static bool HasClaim(string claimName)
    {
        return GetUserIdentity().Claims.Any(x => x.Type == claimName);
    }

    private static bool GetUserIsAuthenticated()
    {
        return GetUserIdentity().IsAuthenticated;
    }

    public static void RemoveUserClaims(string claimName = null)
    {
        if (UserIdentity == null || UserClaims == null)
            return;

        if (!string.IsNullOrEmpty(claimName))
        {
            var claim = UserClaims.FirstOrDefault(x => x.Type == claimName);
            if (claim != null)
                UserIdentity.RemoveClaim(claim);

            return;
        }

        foreach (var claim in UserClaims)
            UserIdentity.TryRemoveClaim(claim);
    }
}