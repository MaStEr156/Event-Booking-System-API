namespace Event_Booking_System_API.BookingService.DTOs
{
    public class AddBookingResponse
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public DateTime BookingDate { get; set; }
    }
} 