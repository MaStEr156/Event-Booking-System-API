using AuthenticationService.Helpers;
using DB_Layer.Entities;
using Event_Booking_System_API.AuthService.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ollama_DB_layer.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Event_Booking_System_API.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JWTManager _jwtManager;
        private readonly JWT _jwt;

        public AuthService(
            UserManager<AppUser> userManager,
            JWTManager jwtManager,
            IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            _jwt = jwt.Value;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new RegisterResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Message = "User registered successfully"
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return null;
            }

            var token = await _jwtManager.CreateJwtToken(user, _userManager);
            var refreshToken = _jwtManager.GenerateRefreshToken();

            // Initialize RefreshTokens list if null
            user.RefreshTokens ??= new List<RefreshToken>();
            
            // Add new refresh token
            user.RefreshTokens.Add(refreshToken);
            
            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                Expiration = token.ValidTo,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        public async Task<string> UpdateProfileAsync(UpdateProfileModel request, string token)
        {
            var principal = _jwtManager.ValidateToken(token);
            if (principal == null)
            {
                return "Invalid token";
            }

            var userId = principal.FindFirst("UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "User not found";
            }

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;
            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;
            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                {
                    return "Current password is incorrect";
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    return string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return string.Join(", ", updateResult.Errors.Select(e => e.Description));
            }

            return "Profile updated successfully";
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            var principal = _jwtManager.ValidateToken(token);
            if (principal == null)
            {
                return "Invalid token";
            }

            var userId = principal.FindFirst("UserId")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshTokens == null)
            {
                return "Invalid refresh token";
            }

            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == token);
            if (refreshToken == null || refreshToken.ExpiresOn <= DateTime.UtcNow)
            {
                return "Invalid refresh token";
            }

            var newToken = await _jwtManager.CreateJwtToken(user, _userManager);
            var newRefreshToken = _jwtManager.GenerateRefreshToken();

            // Remove old refresh token and add new one
            user.RefreshTokens.Remove(refreshToken);
            user.RefreshTokens.Add(newRefreshToken);
            
            await _userManager.UpdateAsync(user);

            return new JwtSecurityTokenHandler().WriteToken(newToken);
        }

        public async Task<string> AssignRoleAsync(RoleModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return "User not found";
            }

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return "Role assigned successfully";
        }

        public async Task<string> DeassignRoleAsync(RoleModel request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return "User not found";
            }

            var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                return string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return "Role deassigned successfully";
        }

        public async Task<AppUser> GetUserByTokenAsync(string token)
        {
            var principal = _jwtManager.ValidateToken(token);
            if (principal == null)
            {
                return null;
            }

            var userId = principal.FindFirst("UserId")?.Value;
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> LoggoutAsync(string refreshtoken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => 
                u.RefreshTokens != null && u.RefreshTokens.Any(rt => rt.Token == refreshtoken));
            
            if (user == null)
            {
                return false;
            }

            var token = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshtoken);
            if (token != null)
            {
                user.RefreshTokens.Remove(token);
                await _userManager.UpdateAsync(user);
            }

            return true;
        }
    }
}
