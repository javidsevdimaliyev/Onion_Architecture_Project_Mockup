namespace SolutionName.Application.Shared.Configurations;

public class AudienceConfig
{
    public string Secret { get; set; }
    public string RefreshTokenSecret { get; set; }
    public string Iss { get; set; }
    public string Aud { get; set; }
    public int Expire { get; set; }
    public int ExpireRefreshToken { get; set; }
    public int ExpireMinRefreshToken { get; set; }

    public int ExpireMinAccessToken { get; set; }

    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    public int RefreshTokenTTL { get; set; }
    public string SecretForTemporaryAccessRequest { get; set; }
    public string TemporaryAccessUrl { get; set; }
    public string CookiePath { get; set; }
}