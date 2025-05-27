using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO.AuthController;

public class LoginOtpRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(100, MinimumLength = 1)]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
