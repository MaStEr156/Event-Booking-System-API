using Event_Booking_System_API.BookingService;
using Event_Booking_System_API.BookingService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // For User.FindFirst

namespace Event_Booking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Typically, booking actions would require authorization
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookingsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            try
            {
                var (bookings, error) = await _bookingService.GetAllBookingsAsync(pageNumber, pageSize);
                if (error != null)
                {
                    return NotFound(error);
                }
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBookingById/{id}")]
        public async Task<IActionResult> GetBookingByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Booking ID cannot be null or empty.");
            }
            try
            {
                var (booking, error) = await _bookingService.GetBookingByIdAsync(id);
                if (error != null) // Or if booking is null and error is set
                {
                    return NotFound(error);
                }
                return Ok(booking);
            }
            catch (KeyNotFoundException ex) // Service might throw this directly if preferred
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddBooking")]
        public async Task<IActionResult> AddBookingAsync([FromBody] BookingRequest bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest("Booking data is null.");
            }

            // --- IMPORTANT: Retrieve UserId from authenticated user context ---
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated or UserId not found.");
            }

            try
            {
                var (addedBooking, error) = await _bookingService.AddBookingAsync(bookingDto, userId);
                if (error != null)
                {
                    // Errors could be "Event not found" or other validation issues from the service
                    return BadRequest(error); 
                }
                // Consider CreatedAtAction if you have a GetBookingById endpoint
                // return CreatedAtAction(nameof(GetBookingByIdAsync), new { id = addedBooking.Id }, addedBooking);
                return Ok(addedBooking);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /*[HttpPut("UpdateBooking/{id}")]
        public async Task<IActionResult> UpdateBookingAsync(string id, [FromBody] UpdateBookingRequest bookingDto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Booking ID cannot be null or empty.");
            }
            if (bookingDto == null)
            {
                return BadRequest("Booking update data is null.");
            }

            // Optional: Check if the user updating is the owner of the booking or has permission
            // var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var bookingToUpdate = await _bookingService.GetBookingByIdAsync(id); // You might need a method that returns the entity or UserId
            // if (bookingToUpdate.Item1 == null || bookingToUpdate.Item1.UserId != currentUserId) // Assuming GetBookingResponse has UserId
            // {
            //     return Forbid(); 
            // }

            try
            {
                var error = await _bookingService.UpdateBookingAsync(id, bookingDto);
                if (error != null)
                {
                    if (error.Contains("not found"))
                        return NotFound(error);
                    return BadRequest(error);
                }
                return Ok(new { Message = "Booking updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }*/

        [HttpDelete("DeleteBooking/{id}")]
        public async Task<IActionResult> DeleteBookingAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Booking ID cannot be null or empty.");
            }
            try
            {
                var error = await _bookingService.DeleteBookingAsync(id);
                if (error != null)
                {
                    return NotFound(error); // Assuming error means "not found" here from service
                }
                return Ok(new { Message = "Booking deleted successfully." });
            }
            catch (KeyNotFoundException ex) // If service throws this
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("SoftDeleteBooking/{id}")] // Or "CancelBooking"
        public async Task<IActionResult> SoftDeleteBookingAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Booking ID cannot be null or empty.");
            }
            try
            {
                var error = await _bookingService.SoftDeleteBookingAsync(id);
                if (error != null)
                {
                    return NotFound(error); // Assuming error means "not found" here
                }
                return Ok(new { Message = "Booking cancelled (soft deleted) successfully." });
            }
            catch (KeyNotFoundException ex) // If service throws this
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserBookings")]
        [Authorize]
        public async Task<IActionResult> GetUserBookingsAsync()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated or UserId not found.");
            }

            try
            {
                var (bookings, error) = await _bookingService.GetBookingsByUserIdAsync(userId);
                if (bookings == null)
                {
                    return NotFound(error);
                }
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // Log exception ex
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
} 