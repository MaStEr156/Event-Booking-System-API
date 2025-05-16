using System.ComponentModel.DataAnnotations;

namespace Event_Booking_System_API.BookingService.DTOs
{
    public class BookingRequest
    {
        [Required]
        public string EventId { get; set; }
        public DateTime BookingDate { get; set; }
    }
} 