using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(Booking booking);
    }

    public class BookingRepository : IBookingRepository
    {
        private UnitOfWork _unitOfWork;

        public BookingRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Booking> GetActiveBookings(Booking booking)
        {
            return _unitOfWork.Query<Booking>().Where(b => b.Id != booking.Id && b.Status != "Cancelled");
        }

    }
}