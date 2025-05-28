using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ManagementAPI.Enums;

namespace ManagementAPI.DTO
{
    public class UserResponseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'admin' or 'user'.")]
        public string Role { get; set; } = "user";
    }
}
