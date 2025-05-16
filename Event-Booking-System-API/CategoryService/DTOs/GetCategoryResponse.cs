namespace Event_Booking_System_API.CategoryService.DTOs
{
    public class GetCategoryResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int EventsCount { get; set; } // Number of events in this category
    }
} 