namespace Backend.Models.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Domain { get; set; }
        public string? Mobile { get; set; }
    }
}