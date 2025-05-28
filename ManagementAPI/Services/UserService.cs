using ManagementAPI.DTO;
using ManagementAPI.Helpers;
using ManagementAPI.Models;
using ManagementAPI.Enums;
using ManagementAPI.Interfaces;

namespace ManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IDefaultUserRepository _defaultUserRepository;
        public UserService(IDefaultUserRepository defaultUserRepository)
        {
            _defaultUserRepository = defaultUserRepository;
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var userTable = await _defaultUserRepository.GetUserByIdAsync(id);
            if (userTable == null) return null;
            return GenerateDefaultUserFromUserTable(userTable);
        }

        public async Task<List<UserResponseDto>> GetListUserAsync(int limit, int page)
        {
            var usersTableList = await _defaultUserRepository.GetUserListAsync(limit, page);
            return usersTableList.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList();
        }

        public async Task<UserServiceResult> CreateUserAsync(CreateUserRequestDto defaultUser)
        {
            /* validate existing username */
            if (await _defaultUserRepository.GetUserByNameAsync(defaultUser.Username) != null)
                return UserServiceResult.UsernameAlreadyExists;

            /* validate existing email */
            if (await _defaultUserRepository.GetUserByEmailAsync(defaultUser.Email) != null)
                return UserServiceResult.EmailAlreadyExists;

            var userEntity = GenerateUserTableFromDefaultUser(defaultUser);
            if (userEntity == null)
                return UserServiceResult.GenerationFailed;

            var createdUser = await _defaultUserRepository.CreateUserAsync(userEntity);
            if (createdUser == null)
                return UserServiceResult.CreationFailed;

            return UserServiceResult.Success;
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserRequestDto defaultUser)
        {
            if (defaultUser == null) return null;

            var userFromTable = await _defaultUserRepository.GetUserByIdAsync(id);
            if (userFromTable == null) return null;

            var updatedUserTable = MapFieldsToChange(userFromTable, defaultUser);

            var userTableUpdated = await _defaultUserRepository.UpdateUser(updatedUserTable);
            if (userTableUpdated == null) return null;

            return GenerateDefaultUserFromUserTable(userTableUpdated);
        }

        public async Task<UserResponseDto?> DeleteUserByIdAsync(int id)
        {
            var userTable = await _defaultUserRepository.GetUserByIdAsync(id);
            if (userTable == null) return null;

            var deletedUserTable = _defaultUserRepository.DeleteUser(userTable);
            if (deletedUserTable == null) return null;

            return GenerateDefaultUserFromUserTable(userTable);
        }

        private UserTable GenerateUserTableFromDefaultUser(CreateUserRequestDto defaultUser)
        {
            return new UserTable
            {
                Username = defaultUser.Username,
                Email = defaultUser.Email,
                Password = PasswordEncryptionHelper.HashPassword(defaultUser.Password),
                Role = defaultUser.Role,
            };
        }

        private UserResponseDto GenerateDefaultUserFromUserTable(UserTable userTable)
        {
            var defaultUser = new UserResponseDto
            {
                Id = userTable.Id,
                Username = userTable.Username,
                Email = userTable.Email,
                Role = userTable.Role,
            };

            return defaultUser;
        }

        private bool ValidateDefaultUser(CreateUserRequestDto defaultUser, bool validateWithPassword = false)
        {
            if (validateWithPassword)
            {
                if (defaultUser == null) return false;
                if (defaultUser.Username == null) return false;
                if (defaultUser.Email == null) return false;
                if (defaultUser.Password == null) return false;
                return true;
            }

            if (defaultUser == null) return false;
            if (defaultUser.Username == null) return false;
            if (defaultUser.Email == null) return false;

            return true;
        }

        private UserTable MapFieldsToChange(UserTable userTable, UpdateUserRequestDto defaultUser)
        {
            if (!string.IsNullOrWhiteSpace(defaultUser.Username) &&
                    defaultUser.Username.Length >= 6 &&
                    defaultUser.Username != userTable.Username)
            {
                userTable.Username = defaultUser.Username;
            }

            if (!string.IsNullOrWhiteSpace(defaultUser.Email) &&
                defaultUser.Email.Length >= 6 &&
                defaultUser.Email != userTable.Email)
            {
                userTable.Email = defaultUser.Email;
            }

            if (!string.IsNullOrWhiteSpace(defaultUser.Password) &&
                defaultUser.Password.Length >= 6 &&
                !PasswordEncryptionHelper.VerifyPassword(defaultUser.Password, userTable.Password))
            {
                userTable.Password = PasswordEncryptionHelper.HashPassword(defaultUser.Password);
            }

            if (!string.IsNullOrWhiteSpace(defaultUser.Role))
            {
                userTable.Role = defaultUser.Role;
            }

            return userTable;
        }
    }
}
