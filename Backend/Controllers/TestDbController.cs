using Backend.Data;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/testdb")]
    public class TestDbController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TestDbController(AppDbContext db) => _db = db;

        [HttpPost("course")]
        public async Task<IActionResult> AddCourse()
        {
            var course = new Course { Title = "Sample Course", Description = "DB working" };
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return Ok(course);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
            => Ok(await _db.Courses.ToListAsync());
    }
}