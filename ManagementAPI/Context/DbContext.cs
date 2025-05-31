using ManagementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagementAPI.Context;

public class DbContext : IdentityDbContext
{

    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    public DbSet<UserTable> User { get; set; } = null!;
}


