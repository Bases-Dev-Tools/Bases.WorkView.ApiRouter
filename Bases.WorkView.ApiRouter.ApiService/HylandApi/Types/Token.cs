using Refit;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Token
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "";

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = "";

    public DateTime Expiration { get; private set; }

    public void SetExpiration()
    {
        Expiration = DateTime.UtcNow.AddSeconds(ExpiresIn);
    }

    public bool IsExpired()
    {
        return Expiration < DateTime.UtcNow;
    }
}
public static class HylandAuth
{
    public static FormUrlEncodedContent Body => new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                { "grant_type", AppVariables.Grant },
                { "scope", AppVariables.Scope },
                { "client_id", AppVariables.ClientId },
                { "client_secret", AppVariables.Secret },
                { "tenant", AppVariables.Tenant },
                { "username", AppVariables.Username },
                { "password", AppVariables.Password },
            });
}