using Backend.Data;
using Backend.Models.DTOs.Courses;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CoursesController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ Admin creates course
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description,
                Url = request.Url
            };

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();

            return Ok(course);
        }

        // ✅ List all courses
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            return Ok(await _db.Courses.ToListAsync());
        }
    }
}