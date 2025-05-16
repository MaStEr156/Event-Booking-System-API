using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.BookingService.DTOs
{
    public class UpdateBookingRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be at least 1.")]
        public int NumberOfTickets { get; set; }
    }
} 