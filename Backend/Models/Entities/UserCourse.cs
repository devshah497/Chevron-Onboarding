namespace Backend.Models.Entities
{
    public class UserCourse
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;  // ApplicationUser.Id (string)
        public int CourseId { get; set; }

        public DateTime AssignedOnUtc { get; set; } = DateTime.UtcNow;
        public DateTime? DeadlineUtc { get; set; }

        public bool Completed { get; set; }
        public DateTime? CompletedOnUtc { get; set; }
        public string? Remarks { get; set; }
    }
}
