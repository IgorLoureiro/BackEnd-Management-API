using ManagementAPI.Models;

namespace ManagementAPI.Repository
{
    public interface IDefaultUserRepository
    {
        Task<UserTable?> GetUserByIdAsync(int id);
        Task<UserTable?> GetUserByNameAsync(string name);
        Task<UserTable?> GetUserByEmailAsync(string email);
        Task<UserTable?> CreateUserAsync(UserTable userTable);
        Task<UserTable?> DeleteUser(UserTable userTable);
        Task<UserTable?> UpdateUser(UserTable userTable);
    }
}
