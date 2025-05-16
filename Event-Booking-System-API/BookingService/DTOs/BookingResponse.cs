namespace Event_Booking_System_API.BookingService.DTOs
{
    public class BookingResponse
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string EventVenue { get; set; }
        public DateTime BookingDate { get; set; }
    }
} 