using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManagementAPI.DTO
{
    public class DefaultUserResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 6)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
    }
}
