namespace JwtAuthDemo2.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
}
