using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO;

public class SendOtpRequestDto
{
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
