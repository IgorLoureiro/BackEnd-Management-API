
namespace ManagementAPI.DTO;

public class NotFoundErrorDto
{
    public int status { get; set; }
    public string message { get; set; } = string.Empty;
    public string error { get; set; } = string.Empty;
}