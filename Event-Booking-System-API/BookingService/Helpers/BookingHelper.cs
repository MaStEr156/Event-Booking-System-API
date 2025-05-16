using DB_Layer.Entities;
using Event_Booking_System_API.BookingService.DTOs;

namespace Event_Booking_System_API.BookingService.Helpers
{
    public static class BookingHelper
    {
        public static void UpdateBookingDetails(this Booking booking, UpdateBookingRequest bookingDto)
        {
            // Only update fields that are present in UpdateBookingRequest
            // and are meant to be updatable.
            // booking.UpdatedAt = DateTime.UtcNow; // If you have an UpdatedAt field
        }
    }
} 