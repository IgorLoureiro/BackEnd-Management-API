using ManagementAPI.Common.Enums;
using ManagementAPI.Models;

namespace ManagementAPI.Helpers
{
    public static class DatabaseSeeder
    {
        const string AdmEmail = "adm@seed.com";
        const string AdmPass = "SecurePass#12";
        const string AdmUserName = "FirsAdm";

        const string UserEmail = "user@seed.com";
        const string UserPass  = "SecurePass#12";
        const string UserName = "FirsUser";

        public static void Seed(ManagementAPI.Context.DbContext context)
        {
            if (!context.User.Any(u => u.Email == AdmEmail))
            {
                context.User.Add(new UserTable
                {
                    Email = AdmEmail,
                    Password = PasswordEncryptionHelper.HashPassword(AdmPass),
                    Username = AdmUserName,
                    Role = UserRole.admin.ToString(),
                });
            }

            if (!context.User.Any(u => u.Email == UserEmail))
            {
                context.User.Add(new UserTable
                {
                    Email = UserEmail,
                    Password = PasswordEncryptionHelper.HashPassword(UserPass),
                    Username = UserName,
                    Role = UserRole.user.ToString(),
                });
            }

            context.SaveChanges();
        }
    }
}
