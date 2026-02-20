namespace Backend.Models.DTOs.Users
{
    public class CreateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? FullName { get; set; }
        public string? Domain { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? Mobile { get; set; }
    }
}