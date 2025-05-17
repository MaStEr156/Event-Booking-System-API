using DB_Layer.UOW;
using Event_Booking_System_API.EventService.DTOs;
using Event_Booking_System_API.EventService.Helpers;
using Event_Booking_System_API.EventService.Mappers;
using Microsoft.AspNetCore.Hosting;

namespace Event_Booking_System_API.EventService
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(IEnumerable<EventResponse>?, string?)> GetAllEventsAsync(int pageNumber, int pageSize)
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync(pageNumber, pageSize);
            if (events == null || !events.Any())
                return (null, "No Events Found");

            try
            {
                var eventResponses = events.ToEventResponses();
                return (eventResponses, null);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}");
            }
        }

        public async Task<(GetEventResponse?, string?)> GetEventByIdAsync(string id)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            try
            {
                var eventResponse = eventEntity.ToGetEventResponse();
                return (eventResponse, null);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}");
            }
        }

        public async Task<(IEnumerable<EventResponse>?, string?)> GetEventsByCategoryAsync(string categoryId, int pageNumber, int pageSize)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByCategoryId(categoryId, pageNumber, pageSize);
            if (events == null)
                return (null, "No Events Found");
            try
            {
                var eventResponses = events.ToEventResponses();
                return (eventResponses, null);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}");
            }
        }

        public async Task<(AddEventResponse?, string?)> AddEventAsync(EventRequest eventDto)
        {
            try
            {
                string imageUrl = await eventDto.Image.SaveEventImageAsync("EventsImages", _webHostEnvironment);
                var eventEntity = eventDto.ToEvent(imageUrl);

                await _unitOfWork.EventRepository.AddAsync(eventEntity);
                await _unitOfWork.SaveChangesAsync();

                return (new AddEventResponse
                {
                    Id = eventEntity.Id,
                    Title = eventEntity.Title
                }, null);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}");
            }
        }

        public async Task UpdateEventAsync(string id, EventRequest eventDto)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            try
            {
                if (eventDto.Image != null)
                {
                    string imageUrl = await eventDto.Image.SaveEventImageAsync("EventsImages", _webHostEnvironment);
                    eventEntity.UpdateEvent(eventDto, imageUrl);
                }
                else
                {
                    eventEntity.UpdateEvent(eventDto, eventEntity.ImageUrl);
                }

                    await _unitOfWork.EventRepository.UpdateAsync(eventEntity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task DeleteEventAsync(string id)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            try
            {
                await _unitOfWork.EventRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async Task SoftDeleteEventAsync(string id)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            try
            {
                await _unitOfWork.EventRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
