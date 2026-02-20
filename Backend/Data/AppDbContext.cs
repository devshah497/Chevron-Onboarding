using Backend.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
        public DbSet<UserCourse> UserCourses { get; set; } = default!;
    }
}