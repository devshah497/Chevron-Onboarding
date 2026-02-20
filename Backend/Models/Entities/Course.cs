using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Url { get; set; }

        // Optional but useful (matches your UI concept of course category)
        public string? Category { get; set; }  // Domain / Process / Technology / POC, etc.

        // Audit
        public string? CreatedByUserId { get; set; } // Admin (Identity user id)
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
