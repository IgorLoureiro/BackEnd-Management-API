using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
