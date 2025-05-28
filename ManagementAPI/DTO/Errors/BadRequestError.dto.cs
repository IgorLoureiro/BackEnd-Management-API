
namespace ManagementAPI.DTO;

public class BadRequestResponseDto
{
    public int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = new();
}