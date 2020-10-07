using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperTests
    {
        private Mock<IBookingRepository> _bookingRepository;
        private BookingHelper _bookingHelper;

        [SetUp]
        public void Setup()
        {
            _bookingRepository = new Mock<IBookingRepository>();
            _bookingHelper = new BookingHelper(_bookingRepository.Object);
        }
        
        [Test]
        public void OverlappingBookingsExist_BookingStatusIsCancelled_ReturnsEmptyString()
        {
            var result = _bookingHelper.OverlappingBookingsExist(new Booking() { Status = "Cancelled" });
            Assert.That(result, Is.EqualTo(""));
        }
        
        [Test]
        public void OverlappingBookingsExist_NoOverlappingBooking_ReturnsEmptyString()
        {
            var result = _bookingHelper.OverlappingBookingsExist(new Booking());
            Assert.That(result, Is.EqualTo(""));
        }
        
        [Test]
        public void OverlappingBookingsExist_OverlappingBookingArrivalDateLessThanDepartureDate_ReturnsBookingReference()
        {
            var newBooking = new Booking(){ ArrivalDate = new DateTime(2020, 1, 1)};
            var existingBooking = new Booking()
            {
                ArrivalDate = new DateTime(2020, 1, 1),
                DepartureDate = new DateTime(2020, 2, 1),
                Reference = "1"
            };
            _bookingRepository.Setup(b => b.GetActiveBookings(newBooking))
                .Returns(new List<Booking>() { existingBooking }.AsQueryable()); //AsQueryable turns a list into an IQueryable<Book>
            
            //act
            var result = _bookingHelper.OverlappingBookingsExist(newBooking);
            
            //assert
            Assert.That(result, Is.EqualTo("1"));
        }
        
        [Test]
        public void OverlappingBookingsExist_OverlappingBookingDepartureDate_ReturnsBookingReference()
        {
            //arrange
            var newBooking = new Booking(){ DepartureDate = new DateTime(2020, 2, 1)};
            var existingBooking = new Booking()
            {
                ArrivalDate = new DateTime(2020, 1, 1),
                DepartureDate = new DateTime(2020, 2, 1),
                Reference = "2"
            };
            _bookingRepository.Setup(b => b.GetActiveBookings(newBooking)).Returns(new List<Booking>() { existingBooking }.AsQueryable());
            
            //act
            var result = _bookingHelper.OverlappingBookingsExist(newBooking);
            
            //assert
            Assert.That(result, Is.EqualTo("2"));
        }
    }
}