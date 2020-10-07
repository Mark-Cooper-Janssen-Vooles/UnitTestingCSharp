using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public interface IHousekeeperHelperRepository
    {
        IQueryable<Housekeeper> GetHouseKeepers();
    }

    public class HousekeeperHelperRepository : IHousekeeperHelperRepository
    {
        private UnitOfWork _unitOfWork;

        public HousekeeperHelperRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IQueryable<Housekeeper> GetHouseKeepers()
        {
            return _unitOfWork.Query<Housekeeper>();
        }
    }
}