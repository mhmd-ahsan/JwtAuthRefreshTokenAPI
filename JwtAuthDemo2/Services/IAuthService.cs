namespace JwtAuthDemo2.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string username, string password, string role = "User");
        Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password);
        Task<(string newAccessToken, string newRefreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}
