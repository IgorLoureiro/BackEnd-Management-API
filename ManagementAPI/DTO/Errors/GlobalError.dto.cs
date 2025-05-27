using System.Text.Json.Serialization;

namespace ManagementAPI.DTO
{

    public class FieldErrorDto
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class GlobalErrorDto
    {
        [JsonPropertyName("errors")]
        public List<FieldErrorDto> Errors { get; set; } = new();
    }

    public class ValidationStyleErrorDto
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; } = new();
    }
}
