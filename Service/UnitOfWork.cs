using BusinessObject.Model;
using Repository.Repository;


namespace Service
{
    public class UnitOfWork
    {
        private UserRepository _userRepository;
        private HairServiceRepository _hairServiceRepository;

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

        public HairServiceRepository HairServiceRepository
        {
            get
            {
                return _hairServiceRepository ??= new HairServiceRepository(_context);
            }
        }
    }
}
