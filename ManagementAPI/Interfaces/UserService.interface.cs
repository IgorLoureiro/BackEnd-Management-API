using ManagementAPI.DTO;
using ManagementAPI.Enums;

namespace ManagementAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserServiceResult> CreateUserAsync(CreateUserRequestDto defaultUser);
        Task<List<UserResponseDto>> GetListUserAsync(int limit, int page);
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserRequestDto defaultUser);
        Task<UserResponseDto?> DeleteUserByIdAsync(int id);
    }
}
