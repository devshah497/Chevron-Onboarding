using Backend.Data;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/my/courses")]
    [Authorize]
    public class UserCoursesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCoursesController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ✅ Employee: view my courses
        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            var user = await _userManager.GetUserAsync(User);

            var courses = await _db.UserCourses
                .Where(x => x.UserId == user!.Id)
                .Join(_db.Courses,
                      uc => uc.CourseId,
                      c => c.CourseId,
                      (uc, c) => new
                      {
                          c.CourseId,
                          c.Title,
                          uc.Completed
                      })
                .ToListAsync();

            return Ok(courses);
        }

        // ✅ Employee: mark course completed
        [HttpPut("{courseId}")]
        public async Task<IActionResult> MarkCompleted(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);

            var record = await _db.UserCourses
                .FirstOrDefaultAsync(x => x.UserId == user!.Id && x.CourseId == courseId);

            if (record == null) return NotFound();

            record.Completed = true;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Course marked as completed" });
        }
    }
}