using DB_Layer.Entities;
using Event_Booking_System_API.AuthService.DTOs;

namespace Event_Booking_System_API.AuthService
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> UpdateProfileAsync(UpdateProfileModel request, string token);
        Task<(RefreshTokenResponse?, string?)> RefreshTokenAsync(RefreshTokenRequest request);
        Task<string> AssignRoleAsync(RoleModel request);
        Task<string> DeassignRoleAsync(RoleModel request);
        Task<UserResponse> GetUserByTokenAsync(string token);
        Task<bool> LogoutAsync(LogoutRequest request);


    }
}
