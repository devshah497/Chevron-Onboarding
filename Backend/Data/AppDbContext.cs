using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses => Set<Course>();
    }
}