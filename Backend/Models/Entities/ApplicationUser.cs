using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Domain { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
