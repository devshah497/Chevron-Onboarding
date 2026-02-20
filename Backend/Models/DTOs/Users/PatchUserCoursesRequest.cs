namespace Backend.Models.DTOs.Users
{
    public class PatchUserCoursesRequest
    {
        public List<int> AddCourseIds { get; set; } = new();
        public List<int> RemoveCourseIds { get; set; } = new();

        public DateTime? DeadlineUtc { get; set; } // optional common deadline
    }
}