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

        public DateTime? DateAssigned { get; set; }

        public DateTime? Deadline { get; set; }

        public int? CreatedBy { get; set; } // AdminId (you can FK later)
    }
}