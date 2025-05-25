using ManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = ManagementAPI.Context.DbContext;

namespace ManagementAPI.Repository
{
    public class DefaultUserRepository : IDefaultUserRepository
    {
        private readonly DbContext _dbContext;

        public DefaultUserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserTable?> GetUserByIdAsync(int id)
        {
            return await _dbContext.User.FindAsync(id);
        }

        public async Task<UserTable?> GetUserByNameAsync(string name)
        {
            return await _dbContext.User.FirstOrDefaultAsync(ut => ut.Username.Equals(name));
        }

        public async Task<UserTable?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.User.FirstOrDefaultAsync(ut => ut.Email.Equals(email));
        }

        public async Task<UserTable?> CreateUserAsync(UserTable userTable)
        {
            await _dbContext.User.AddAsync(userTable);
            await _dbContext.SaveChangesAsync();
            return userTable;
        }

        public async Task<UserTable?> DeleteUser(UserTable userTable)
        {
            _dbContext.User.Remove(userTable);
            await _dbContext.SaveChangesAsync();
            return userTable;
        }

        public async Task<UserTable?> UpdateUser(UserTable userTable)
        {
            _dbContext.User.Update(userTable);
            await _dbContext.SaveChangesAsync();
            return userTable;
        }
    }
}
