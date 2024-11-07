using Repository.IRepository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IHairServiceRepository HairServiceRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IScheduleUserRepository ScheduleUserRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IBookingRepository BookingRepository { get; }
        IReportRepository ReportRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        IServicesStylistRepository ServicesStylistRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IBookingDetailRepository BookingDetailRepository { get; }
    }
}
