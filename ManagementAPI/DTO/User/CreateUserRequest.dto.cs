using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ManagementAPI.Common.Enums;

namespace ManagementAPI.DTO
{
    public class CreateUserRequestDto
    {
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("role")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'admin' or 'user'.")]
        public string Role { get; set; } = "user";
    }
}
