using ManagementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Context;

 public class DbContext(DbContextOptions<DbContext> options) : IdentityDbContext(options)
{
    public DbSet<UserTable> User { get; set; } = null!;
}


