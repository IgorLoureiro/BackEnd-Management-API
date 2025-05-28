namespace ManagementAPI.Models;

public class UserTable
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? PasswordRecovery { get; set; }
    public string? OtpCode { get; set; }
    public DateTime? OtpExpiration { get; set; }
}