using Backend.Data;
using Backend.Models.DTOs.Users;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
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

        // =========================
        // EMPLOYEE: My assigned courses
        // GET /api/v1/my/courses
        // =========================
        [HttpGet("api/v1/my/courses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _db.UserCourses
                .Where(uc => uc.UserId == user.Id)
                .Join(_db.Courses,
                    uc => uc.CourseId,
                    c => c.CourseId,
                    (uc, c) => new
                    {
                        c.CourseId,
                        c.Title,
                        c.Description,
                        c.Url,
                        c.Category,
                        uc.AssignedOnUtc,
                        uc.DeadlineUtc,
                        uc.Completed,
                        uc.CompletedOnUtc,
                        uc.Remarks
                    })
                .AsNoTracking()
                .ToListAsync();

            return Ok(result);
        }

        // =========================
        // ADMIN: View employee assigned courses
        // GET /api/v1/employees/{id}/courses
        // (Employee can also call ONLY for self)
        // =========================
        [HttpGet("api/v1/employees/{id}/courses")]
        public async Task<IActionResult> GetEmployeeCourses(string id)
        {
            var caller = await _userManager.GetUserAsync(User);
            if (caller == null) return Unauthorized();

            var callerRoles = await _userManager.GetRolesAsync(caller);
            var isAdmin = callerRoles.Contains("Admin");

            if (!isAdmin && caller.Id != id)
                return Forbid();

            var result = await _db.UserCourses
                .Where(uc => uc.UserId == id)
                .Join(_db.Courses,
                    uc => uc.CourseId,
                    c => c.CourseId,
                    (uc, c) => new
                    {
                        c.CourseId,
                        c.Title,
                        c.Description,
                        c.Url,
                        c.Category,
                        uc.AssignedOnUtc,
                        uc.DeadlineUtc,
                        uc.Completed,
                        uc.CompletedOnUtc,
                        uc.Remarks
                    })
                .AsNoTracking()
                .ToListAsync();

            return Ok(result);
        }

        // =========================
        // ADMIN: Assign / Unassign courses (PATCH)
        // PATCH /api/v1/employees/{id}/courses
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPatch("api/v1/employees/{id}/courses")]
        public async Task<IActionResult> PatchEmployeeCourses(string id, PatchUserCoursesRequest request)
        {
            // validate employee exists
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null) return NotFound("Employee not found");

            // Add
            if (request.AddCourseIds.Any())
            {
                var existingCourseIds = await _db.UserCourses
                    .Where(x => x.UserId == id)
                    .Select(x => x.CourseId)
                    .ToListAsync();

                var toAdd = request.AddCourseIds.Distinct()
                    .Where(cid => !existingCourseIds.Contains(cid))
                    .ToList();

                // validate course existence
                var validCourseIds = await _db.Courses
                    .Where(c => toAdd.Contains(c.CourseId))
                    .Select(c => c.CourseId)
                    .ToListAsync();

                foreach (var courseId in validCourseIds)
                {
                    _db.UserCourses.Add(new UserCourse
                    {
                        UserId = id,
                        CourseId = courseId,
                        AssignedOnUtc = DateTime.UtcNow,
                        DeadlineUtc = request.DeadlineUtc,
                        Completed = false
                    });
                }
            }

            // Remove
            if (request.RemoveCourseIds.Any())
            {
                var removeSet = request.RemoveCourseIds.Distinct().ToHashSet();
                var rows = await _db.UserCourses
                    .Where(x => x.UserId == id && removeSet.Contains(x.CourseId))
                    .ToListAsync();

                _db.UserCourses.RemoveRange(rows);
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Courses updated for employee" });
        }

        // =========================
        // EMPLOYEE (self) or ADMIN: update completion true/false
        // PUT /api/v1/employees/{id}/courses/{courseId}
        // =========================
        [HttpPut("api/v1/employees/{id}/courses/{courseId:int}")]
        public async Task<IActionResult> UpdateCompletion(string id, int courseId, UpdateCourseCompletionRequest request)
        {
            var caller = await _userManager.GetUserAsync(User);
            if (caller == null) return Unauthorized();

            var callerRoles = await _userManager.GetRolesAsync(caller);
            var isAdmin = callerRoles.Contains("Admin");

            if (!isAdmin && caller.Id != id)
                return Forbid();

            var record = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == id && x.CourseId == courseId);
            if (record == null) return NotFound("Course not assigned to this employee");

            record.Completed = request.Completed;
            record.CompletedOnUtc = request.Completed ? (request.CompletedOnUtc ?? DateTime.UtcNow) : null;
            record.Remarks = request.Completed ? request.Remarks : null;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Course completion updated" });
        }

        // =========================
        // ADMIN: quick progress summary (tracking)
        // GET /api/v1/employees/{id}/courses/progress
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpGet("api/v1/employees/{id}/courses/progress")]
        public async Task<IActionResult> GetEmployeeProgress(string id)
        {
            var total = await _db.UserCourses.CountAsync(x => x.UserId == id);
            var completed = await _db.UserCourses.CountAsync(x => x.UserId == id && x.Completed);
            return Ok(new { totalAssigned = total, completed, pending = total - completed });
        }
    }
}