using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ManagementAPI.Common.Enums;

namespace ManagementAPI.DTO
{
    public class UpdateUserRequestDto
    {
        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("role")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'admin' or 'user'.")]
        public string? Role { get; set; } = "user";
    }
}
