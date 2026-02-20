using Backend.Models.DTOs.Users;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // ✅ Admin: list all employees
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var users = await _userManager.Users.ToListAsync();

            // Filter only those in Employee role
            var employees = new List<object>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains("Employee"))
                {
                    employees.Add(new
                    {
                        u.Id,
                        u.Email,
                        u.FullName,
                        u.Domain,
                        u.JoiningDate,
                        Mobile = u.PhoneNumber
                    });
                }
            }

            return Ok(employees);
        }

        // ✅ Admin: create employee
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateUserRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Domain = request.Domain,
                JoiningDate = request.JoiningDate,
                PhoneNumber = request.Mobile
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Employee");
            return Ok(new { message = "Employee created successfully", userId = user.Id });
        }

        // ✅ Admin: get employee by id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Employee")) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Domain,
                user.JoiningDate,
                Mobile = user.PhoneNumber
            });
        }

        // ✅ Admin: update employee profile (spec PUT /employees/{id})
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, CreateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.Email = request.Email;
            user.UserName = request.Email;
            user.FullName = request.FullName;
            user.Domain = request.Domain;
            user.JoiningDate = request.JoiningDate;
            user.PhoneNumber = request.Mobile;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "Employee updated successfully" });
        }

        // ✅ Admin: delete employee
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "Employee deleted" });
        }

        // ✅ Employee: get my profile (spec says employee can maintain profile)
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Domain,
                user.JoiningDate,
                Mobile = user.PhoneNumber,
                Role = roles.FirstOrDefault()
            });
        }
    }
}