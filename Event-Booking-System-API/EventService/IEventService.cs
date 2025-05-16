using Event_Booking_System_API.EventService.DTOs;

namespace Event_Booking_System_API.EventService
{
    public interface IEventService
    {
        Task<(IEnumerable<EventResponse>?, string?)> GetAllEventsAsync(int pageNumber, int pageSize);
        Task<(GetEventResponse?, string?)> GetEventByIdAsync(string id);
        Task<(IEnumerable<EventResponse>?, string?)> GetEventsByCategoryAsync(string categoryId, int pageNumber, int pageSize);
        Task<(AddEventResponse?, string?)> AddEventAsync(EventRequest eventDto);
        Task UpdateEventAsync(string id, EventRequest eventDto);
        Task DeleteEventAsync(string id);
        Task SoftDeleteEventAsync(string id);
    }
}
