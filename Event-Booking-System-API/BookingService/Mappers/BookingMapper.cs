using DB_Layer.Entities;
using Event_Booking_System_API.BookingService.DTOs;

namespace Event_Booking_System_API.BookingService.Mappers
{
    public static class BookingMapper
    {
        public static BookingResponse ToBookingResponse(this Booking booking)
        {
            return new BookingResponse
            {
                Id = booking.Id,
                EventId = booking.EventId,
                EventTitle = booking.Event?.Title ?? "N/A",
                EventDate = booking.Event?.EventDate ?? default(DateTime),
                EventVenue = booking.Event?.Venue ?? "N/A",
                BookingDate = booking.BookingDate // Assuming CreatedAt is the booking date
            };
        }

        public static IEnumerable<BookingResponse> ToBookingResponses(this IEnumerable<Booking> bookings)
        {
            return bookings.Select(b => b.ToBookingResponse());
        }

        public static GetBookingResponse ToGetBookingResponse(this Booking booking)
        {
            return new GetBookingResponse
            {
                Id = booking.Id,
                EventId = booking.EventId,
                EventTitle = booking.Event?.Title ?? "N/A",
                EventEventDate = booking.Event?.EventDate ?? default(DateTime),
                EventVenue = booking.Event?.Venue ?? "N/A",
                BookingDate = booking.BookingDate // Assuming CreatedAt is the booking date
            };
        }

        public static Booking ToBooking(this BookingRequest bookingRequest, string userId) // Assuming eventId is confirmed valid before this call
        {
            return new Booking
            {
                EventId = bookingRequest.EventId,
                UserId = userId,
                BookingDate = bookingRequest.BookingDate,
            };
        }

        public static AddBookingResponse ToAddBookingResponse(this Booking booking)
        {
            return new AddBookingResponse
            {
                Id = booking.Id,
                EventId = booking.EventId,
                BookingDate = booking.BookingDate // Assuming CreatedAt is the booking date
            };
        }
    }
} 