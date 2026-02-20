using Backend.Data;
using Backend.Models.DTOs.Courses;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ✅ Everyone logged-in can view courses (Admin + Employee)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCourses()
            => Ok(await _db.Courses.AsNoTracking().ToListAsync());

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _db.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.CourseId == id);
            return course == null ? NotFound() : Ok(course);
        }

        // ✅ Admin creates course
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest request)
        {
            var user = await _userManager.GetUserAsync(User);

            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                Url = request.Url,
                Category = request.Category,
                CreatedByUserId = user?.Id
            };

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
        }

        // ✅ Admin edits course
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseRequest request)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            course.Title = request.Title;
            course.Description = request.Description;
            course.Url = request.Url;
            course.Category = request.Category;
            course.UpdatedAtUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(course);
        }

        // ✅ Admin deletes course
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null) return NotFound();

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Course deleted" });
        }
    }
}