using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ManagementAPI.Enums;

namespace ManagementAPI.DTO
{
    public class DefaultUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 1)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 1)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'admin' or 'user'.")]
        public string Role { get; set; } = "user";
    }
}
