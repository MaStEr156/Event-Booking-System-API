using Event_Booking_System_API.EventService;
using Event_Booking_System_API.EventService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event_Booking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllEvents")]
        public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            try
            {
                var response = await _eventService.GetAllEventsAsync(pageNumber, pageSize);
                if (response.Item1 == null)
                {
                    return NotFound(response.Item2);
                }
                return Ok(response.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetEventById/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var response = await _eventService.GetEventByIdAsync(id);
                if (response.Item1 == null)
                {
                    return NotFound(response.Item2);
                }
                return Ok(response.Item1);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetEventsByCategory/{categoryId}")]
        public async Task<IActionResult> GetEventsByCategoryAsync(string categoryId, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            try
            {
                var response = await _eventService.GetEventsByCategoryAsync(categoryId, pageNumber, pageSize);
                if (response.Item1 == null)
                {
                    return NotFound(response.Item2);
                }
                return Ok(response.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddEvent")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddEventAsync([FromForm] EventRequest eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest("Event data is null.");
            }
            try
            {
                var response = await _eventService.AddEventAsync(eventDto);
                if (response.Item1 == null)
                {
                    return BadRequest(response.Item2);
                }
                return Ok(response.Item1);
                //return CreatedAtAction(nameof(GetByIdAsync), new { id = response.Item1.Id }, response.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateEvent/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEventAsync(string id, [FromForm] EventRequest eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest("Event data is null.");
            }
            try
            {
                await _eventService.UpdateEventAsync(id, eventDto);
                return Ok(new {Message = "Updated Successfully"});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEventAsync(string id)
        {
            try
            {
                await _eventService.DeleteEventAsync(id);
                return Ok(new { Message = "Deleted Successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("SoftDeleteEvent/{id}")]
        public async Task<IActionResult> SoftDeleteEventAsync(string id)
        {
            try
            {
                await _eventService.SoftDeleteEventAsync(id);
                return Ok(new { Message = "Soft Deleted Successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
