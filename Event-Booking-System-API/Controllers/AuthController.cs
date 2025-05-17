using DB_Layer.Entities;
using Event_Booking_System_API.AuthService;
using Event_Booking_System_API.AuthService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event_Booking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.Message != "User registered successfully")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("update-profile")]
        public async Task<ActionResult<string>> UpdateProfile(UpdateProfileModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var response = await _authService.UpdateProfileAsync(request, token);
            if (response.StartsWith("Invalid") || response.StartsWith("User not found"))
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            if (response.Item1 == null)
            {
                return BadRequest(response.Item2);
            }
            return Ok(response.Item1);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<ActionResult<string>> AssignRole(RoleModel request)
        {
            var response = await _authService.AssignRoleAsync(request);
            if (response.StartsWith("User not found"))
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deassign-role")]
        public async Task<ActionResult<string>> DeassignRole(RoleModel request)
        {
            var response = await _authService.DeassignRoleAsync(request);
            if (response.StartsWith("User not found"))
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<ActionResult<AppUser>> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserByTokenAsync(token);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Logout([FromBody] LogoutRequest request)
        {
            var result = await _authService.LogoutAsync(request);
            if (!result)
            {
                return BadRequest("Invalid refresh token");
            }
            return Ok(result);
        }
    }
}
