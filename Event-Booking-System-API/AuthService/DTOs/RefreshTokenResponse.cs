namespace Event_Booking_System_API.AuthService.DTOs
{
    public class RefreshTokenResponse
    {
        public string newAccessToken { get; set; }
        public string newRefreshToken { get; set; }
    }
}
