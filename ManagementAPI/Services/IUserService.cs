using ManagementAPI.DTO;

namespace ManagementAPI.Services
{
    public interface IUserService
    {
        Task<DefaultUser?> GetUserByIdAsync(int id);
        Task<UserServiceResult> CreateUserAsync(DefaultUser defaultUser);
        Task<DefaultUser?> UpdateUserAsync(int id, DefaultUser defaultUser);
        Task<DefaultUser?> DeleteUserByIdAsync(int id);
    }
}
