using DB_Layer.UOW;
using Event_Booking_System_API.BookingService.DTOs;
using Event_Booking_System_API.BookingService.Mappers;
using Event_Booking_System_API.BookingService.Helpers;
using DB_Layer.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Event_Booking_System_API.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<BookingResponse>?, string?)> GetAllBookingsAsync(int pageNumber, int pageSize)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(pageNumber, pageSize);
            if (bookings == null || !bookings.Any())
                return (null, "No Bookings Found");

            try
            {
                var bookingResponses = bookings.ToBookingResponses();
                return (bookingResponses, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error retrieving bookings: {ex.Message}");
            }
        }

        public async Task<(GetBookingResponse?, string?)> GetBookingByIdAsync(string id)
        {
            var bookingEntity = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (bookingEntity == null)
                return (null, $"Booking with ID {id} not found.");

            try
            {
                var bookingResponse = bookingEntity.ToGetBookingResponse();
                return (bookingResponse, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error retrieving booking: {ex.Message}");
            }
        }

        [Authorize]
        public async Task<(AddBookingResponse?, string?)> AddBookingAsync(BookingRequest bookingDto, string userId)
        {
            try
            {
                // Check if user has already booked this event
                var hasExistingBooking = await _unitOfWork.BookingRepository.HasUserBookedEventAsync(userId, bookingDto.EventId);
                if (hasExistingBooking)
                {
                    return (null, "You have already booked this event.");
                }

                // Optional: Validate EventId - Check if the event exists
                var eventEntity = await _unitOfWork.EventRepository.GetByIdAsync(bookingDto.EventId);
                if (eventEntity == null)
                {
                    return (null, $"Event with ID {bookingDto.EventId} not found.");
                }

                var bookingEntity = bookingDto.ToBooking(userId);

                await _unitOfWork.BookingRepository.AddAsync(bookingEntity);
                await _unitOfWork.SaveChangesAsync();

                var response = bookingEntity.ToAddBookingResponse();
                return (response, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error adding booking: {ex.Message}");
            }
        }

        public async Task<string?> UpdateBookingAsync(string id, UpdateBookingRequest bookingDto)
        {
            var bookingEntity = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (bookingEntity == null)
                return $"Booking with ID {id} not found.";

            try
            {
                // Optional: Add validation logic here, e.g., check if the event associated with the booking still allows updates.
                bookingEntity.UpdateBookingDetails(bookingDto); // Using the helper method
                await _unitOfWork.BookingRepository.UpdateAsync(bookingEntity);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error updating booking: {ex.Message}";
            }
        }

        public async Task<string?> DeleteBookingAsync(string id) // Hard delete
        {
            var bookingEntity = await _unitOfWork.BookingRepository.GetByIdAsync(id); // Check if exists first
            if (bookingEntity == null) 
                 return $"Booking with ID {id} not found.";

            try
            {
                await _unitOfWork.BookingRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (KeyNotFoundException knfEx)
            {
                return knfEx.Message; // Repository throws this if find fails during delete op
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error deleting booking: {ex.Message}";
            }
        }

        public async Task<string?> SoftDeleteBookingAsync(string id) // Soft delete
        {
            var bookingEntity = await _unitOfWork.BookingRepository.GetByIdAsync(id); // Check if exists first
            if (bookingEntity == null)
                 return $"Booking with ID {id} not found.";
            try
            {
                await _unitOfWork.BookingRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return null; // Success
            }
            catch (KeyNotFoundException knfEx)
            {
                return knfEx.Message; // Repository throws this if find fails during soft delete op
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return $"Error soft-deleting booking: {ex.Message}";
            }
        }

        public async Task<(IEnumerable<BookingResponse>?, string?)> GetBookingsByUserIdAsync(string userId)
        {
            try
            {
                var bookings = await _unitOfWork.BookingRepository.GetBookingsByUserIdAsync(userId);
                if (bookings == null || !bookings.Any())
                    return ([], "No bookings found for this user");

                var bookingResponses = bookings.ToBookingResponses();
                return (bookingResponses, null);
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return (null, $"Error retrieving user bookings: {ex.Message}");
            }
        }
    }
}
