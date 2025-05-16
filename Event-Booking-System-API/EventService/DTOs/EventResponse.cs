namespace Event_Booking_System_API.EventService.DTOs
{
    public class EventResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
