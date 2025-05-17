using Event_Booking_System_API.BookingService.DTOs;

namespace Event_Booking_System_API.BookingService
{
    public interface IBookingService
    {
        Task<(IEnumerable<BookingResponse>?, string?)> GetAllBookingsAsync(int pageNumber, int pageSize);
        Task<(GetBookingResponse?, string?)> GetBookingByIdAsync(string id);
        Task<(IEnumerable<BookingResponse>?, string?)> GetBookingsByUserIdAsync(string userId);
        Task<(AddBookingResponse?, string?)> AddBookingAsync(BookingRequest bookingDto, string userId);
        Task<string?> UpdateBookingAsync(string id, UpdateBookingRequest bookingDto);
        Task<string?> DeleteBookingAsync(string id); // Hard delete
        Task<string?> SoftDeleteBookingAsync(string id); // Soft delete
    }
}
