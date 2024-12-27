using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ManagementAPI.DTO;

public class SignUp
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}

public class RecoverPassword
{
    public int? Attempts { get; set; } = 0;
    [JsonProperty("RecoveryCode")]
    public string? Code { get; set; } = string.Empty;
}

public class RequestRecover
{
    public string? email { get; set; }
    public string? password { get; set; }
    public string? confirmPassword { get; set; }
    public string? code { get; set; }
}