using DB_Layer.Entities;
using Event_Booking_System_API.EventService.DTOs;

namespace Event_Booking_System_API.EventService.Mappers
{
    public static class EventMapper
    {
        public static IEnumerable<EventResponse> ToEventResponses(this IEnumerable<Event> events)
        {
            return events.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Price = e.Price,
                ImageUrl = e.ImageUrl
            });
        }

        public static GetEventResponse ToGetEventResponse(this Event eventEntity)
        {
            return new GetEventResponse
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                CategoryId = eventEntity.CategoryId,
                CategoryName = eventEntity.Category.Name,
                EventDate = eventEntity.EventDate,
                Venue = eventEntity.Venue,
                Price = eventEntity.Price,
                ImageUrl = eventEntity.ImageUrl
            };
        }

        public static Event ToEvent(this EventRequest createEventRequest, string ImageUrl)
        {
            return new Event
            {
                Title = createEventRequest.Title,
                Description = createEventRequest.Description,
                CategoryId = createEventRequest.CategoryId,
                EventDate = createEventRequest.EventDate,
                Venue = createEventRequest.Venue,
                Price = createEventRequest.Price ?? 0,
                ImageUrl = ImageUrl,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
