using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.IService
{
    public interface IScheduleUserService
    {
        Task<List<ScheduleUserDTO>> GetListScheduleUserAsync();
        Task<ResponseDTO> GetScheduleUserOfCurrentUser();
        Task<ResponseDTO> createScheduleUser(createScheduleUser request);
        Task<ResponseDTO> GetSchedulesOfStylistsAsync();
        Task<ResponseDTO> GetStylistsByScheduleAsync(getStartDateAndStartTime request);
        Task<ResponseDTO> GetSchedulesOfNoStylistAssignAsync();
        Task<ResponseDTO> UpdateScheduleUserAsync(int scheduleId);
        Task<ResponseDTO> GetSchedulesOfCurrentStylistAsync();
    }
}
