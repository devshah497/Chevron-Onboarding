namespace Backend.Models.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;   // from forgot-password
        public string NewPassword { get; set; } = string.Empty;
    }
}