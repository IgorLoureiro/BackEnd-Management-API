using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO.AuthController;

public class SendOtpRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(100, MinimumLength = 1)]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
