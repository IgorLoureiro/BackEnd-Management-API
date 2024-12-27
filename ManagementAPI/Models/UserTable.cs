namespace ManagementAPI.Models;

public class UserTable
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string? PasswordRecovery { get; set; }
}