using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO;
public class LoginOtpRequestDto
{
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
