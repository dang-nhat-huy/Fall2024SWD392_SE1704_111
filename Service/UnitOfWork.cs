using BusinessObject.Model;
using Repository.IRepository;
using Repository.Repository;


namespace Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private IUserRepository _userRepository;
        private IHairServiceRepository _hairServiceRepository;
        private IScheduleRepository _scheduleRepository;
        private IScheduleUserRepository _scheduleUserRepository;
        private IUserProfileRepository _userProfileRepository;

        private HairSalonBookingContext _context;

        public UnitOfWork()
        {
            _userRepository ??= new UserRepository();
            _hairServiceRepository ??= new HairServiceRepository();
            _context ??= new HairSalonBookingContext();
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public IHairServiceRepository HairServiceRepository
        {
            get
            {
                return _hairServiceRepository ??= new HairServiceRepository(_context);
            }
        }

        public IScheduleRepository ScheduleRepository
        {
            get
            {
                return _scheduleRepository ??= new ScheduleRepository(_context);
            }
        }

        public IScheduleUserRepository scheduleUserRepository
        {
            get
            {
                return _scheduleUserRepository ??= new ScheduleUserRepository(_context);
            }
        }

        public IUserProfileRepository userProfileRepository
        {
            get {
                return _userProfileRepository ??= new UserProfileRepository (_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
