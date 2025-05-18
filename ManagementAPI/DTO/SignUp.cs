using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace ManagementAPI.DTO;

public class SignUp
{
    [Required]
    [EmailAddress]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 6)]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}

public class RecoverPassword
{
    public int? Attempts { get; set; } = 0;

    [Required]
    [StringLength(50)]
    [JsonPropertyName("username")]
    [JsonProperty("RecoveryCode")]
    public string? Code { get; set; } = string.Empty;
}

public class RequestRecover
{

    [Required]
    [EmailAddress]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("email")]
    public string? email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("password")]
    public string? password { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("confirmPassword")]
    public string? confirmPassword { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [JsonPropertyName("code")]
    public string? code { get; set; }
}