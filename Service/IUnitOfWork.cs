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
        IScheduleUserRepository scheduleUserRepository { get; }

        IUserProfileRepository userProfileRepository { get; }
    }
}
