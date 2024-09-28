
using BusinessObject.Model;
using Repository.Repository;


namespace BadmintonRentingData
{
    public class UnitOfWork
    {
        private UserRepository _userRepository;   

        private HairSalonBookingContext _context;

        public UnitOfWork()
        {
            _userRepository ??= new UserRepository();
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        
    }
}
