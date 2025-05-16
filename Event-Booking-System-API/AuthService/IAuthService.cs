using DB_Layer.Entities;
using Event_Booking_System_API.AuthService.DTOs;

namespace Event_Booking_System_API.AuthService
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> UpdateProfileAsync(UpdateProfileModel request, string token);
        Task<string> RefreshTokenAsync(string token);
        Task<string> AssignRoleAsync(RoleModel request);
        Task<string> DeassignRoleAsync(RoleModel request);
        Task<AppUser> GetUserByTokenAsync(string token);
        Task<bool> LoggoutAsync(string refreshtoken);


    }
}
