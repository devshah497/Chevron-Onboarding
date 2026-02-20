namespace Backend.Models.DTOs.Courses
{
    public class UpdateCourseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Category { get; set; }
    }
}