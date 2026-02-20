namespace Backend.Models.DTOs.Users
{
    public class UpdateCourseCompletionRequest
    {
        public bool Completed { get; set; }
        public DateTime? CompletedOnUtc { get; set; }
        public string? Remarks { get; set; }
    }
}
