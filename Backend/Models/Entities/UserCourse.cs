namespace Backend.Models.Entities
{
    public class UserCourse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public bool Completed { get; set; }
    }
}