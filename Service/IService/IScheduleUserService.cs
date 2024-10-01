using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.IService
{
    public interface IScheduleUserService
    {
        Task<PagedResult<ScheduleUserDTO>> GetListScheduleUserAsync(int pageNumber, int pageSize);
    }
}
