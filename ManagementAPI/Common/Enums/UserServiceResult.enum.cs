namespace ManagementAPI.Enums;

public enum UserServiceResult
{
    Success,
    InvalidUser,
    UsernameAlreadyExists,
    EmailAlreadyExists,
    GenerationFailed,
    CreationFailed,
}