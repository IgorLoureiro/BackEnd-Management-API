using ManagementAPI.Models;

namespace ManagementAPI.Interfaces
{
    public interface IDefaultUserRepository
    {

        Task<UserTable?> CreateUserAsync(UserTable userTable);
        Task<List<UserTable>> GetUserListAsync(int limit, int page);
        Task<UserTable?> GetUserByIdAsync(int id);
        Task<UserTable?> GetUserByNameAsync(string name);
        Task<UserTable?> GetUserByEmailAsync(string email);
        Task<UserTable?> UpdateUser(UserTable userTable);
        Task<UserTable?> DeleteUser(UserTable userTable);
    }
}
