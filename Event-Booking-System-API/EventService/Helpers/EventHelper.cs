using DB_Layer.Entities;
using Event_Booking_System_API.EventService.DTOs;

namespace Event_Booking_System_API.EventService.Helpers
{
    public static class EventHelper
    {
        public static void UpdateEvent(this Event eventEntity, EventRequest eventDto, string newImageUrl)
        {
            eventEntity.Title = eventDto.Title;
            eventEntity.Description = eventDto.Description;
            eventEntity.CategoryId = eventDto.CategoryId;
            eventEntity.EventDate = eventDto.EventDate;
            eventEntity.Venue = eventDto.Venue;
            eventEntity.Price = eventDto.Price?? 0;
            eventEntity.ImageUrl = newImageUrl ?? "";
        }
    }
} 