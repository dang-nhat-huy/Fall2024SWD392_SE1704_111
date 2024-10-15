using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
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
        private IBookingRepository _bookingRepository;
        private IReportRepository _reportRepository;
        private IVoucherRepository _voucherRepository;
        private IServicesStylistRepository _servicesStylistRepository;

        private HairSalonBookingContext _context;

        public UnitOfWork()
        {
            _userRepository ??= new UserRepository();
            _hairServiceRepository ??= new HairServiceRepository();
            _context ??= new HairSalonBookingContext();
            _scheduleRepository ??= new ScheduleRepository();
            _bookingRepository ??= new BookingRepository();
            _reportRepository ??= new ReportRepository();
            _servicesStylistRepository ??= new ServicesStylistRepository();
            _voucherRepository ??= new VoucherRepository();
            _userProfileRepository ??= new UserProfileRepository();
            _scheduleUserRepository ??= new ScheduleUserRepository();
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

        public IScheduleUserRepository ScheduleUserRepository
        {
            get
            {
                return _scheduleUserRepository ??= new ScheduleUserRepository(_context);
            }
        }

        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                return _userProfileRepository ??= new UserProfileRepository(_context);
            }
        }

        public IBookingRepository BookingRepository
        {
            get
            {
                return _bookingRepository ??= new BookingRepository(_context);
            }
        }

        public IReportRepository ReportRepository
        {
            get
            {
                return _reportRepository ??= new ReportRepository(_context);
            }
        }

        public IVoucherRepository VoucherRepository
        {
            get
            {
                return _voucherRepository ??= new VoucherRepository(_context);
            }
        }

        public IServicesStylistRepository ServicesStylistRepository
        {
            get
            {
                return _servicesStylistRepository ??= new ServicesStylistRepository(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
