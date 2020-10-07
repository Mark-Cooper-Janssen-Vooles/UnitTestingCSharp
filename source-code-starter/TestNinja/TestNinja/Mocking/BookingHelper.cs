using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public class BookingHelper
    {
        private IBookingRepository _bookingRepository;

        public BookingHelper(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public string OverlappingBookingsExist(Booking booking)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var bookings = _bookingRepository.GetActiveBookings(booking);
            
            var overlappingBooking = bookings.FirstOrDefault( b =>
                        booking.ArrivalDate < b.DepartureDate
                        && booking.ArrivalDate < b.DepartureDate
                        || booking.DepartureDate > b.ArrivalDate
                        && booking.DepartureDate <= b.DepartureDate);

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }

    public class UnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }

    public class Booking
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}