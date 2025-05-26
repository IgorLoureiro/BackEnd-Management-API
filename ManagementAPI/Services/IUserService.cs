using ManagementAPI.DTO;

namespace ManagementAPI.Services
{
    public interface IUserService
    {
        Task<DefaultUserResponse?> GetUserByIdAsync(int id);
        Task<UserServiceResult> CreateUserAsync(DefaultUserResponse defaultUser);
        Task<DefaultUserResponse?> UpdateUserAsync(int id, DefaultUserRequest defaultUser);
        Task<DefaultUserResponse?> DeleteUserByIdAsync(int id);
        Task<List<DefaultUserResponse>> GetListUserAsync(int usersPerPage, int page);
    }
}
