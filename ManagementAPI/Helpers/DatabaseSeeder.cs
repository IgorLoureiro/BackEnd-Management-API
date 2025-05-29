using ManagementAPI.Models;

namespace ManagementAPI.Helpers
{
    public static class DatabaseSeeder
    {
        public static void Seed(ManagementAPI.Context.DbContext context)
        {
            if (!context.User.Any(u => u.Email == "adm@seed.com"))
            {
                context.User.Add(new UserTable
                {
                    Email = "adm@seed.com",
                    Password = PasswordEncryptionHelper.HashPassword("SecurePass#12"),
                    Username = "FirsAdm",
                    Role = "admin"
                });
            }

            if (!context.User.Any(u => u.Email == "user@seed.com"))
            {
                context.User.Add(new UserTable
                {
                    Email = "user@seed.com",
                    Password = PasswordEncryptionHelper.HashPassword("SecurePass#12"),
                    Username = "FirstUser",
                    Role = "user"
                });
            }

            context.SaveChanges();
        }
    }
}
