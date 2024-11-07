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
    public interface IScheduleService
    {
        Task<ResponseDTO> GetListScheduleAsync();
         
        Task<ResponseDTO> CreateSchedule(CreateScheduleDTO createScheduleDTO);
        Task<ResponseDTO> UpdateSchedule(UpdateScheduleDTO updateScheduleDTO, int scheduleId);
        Task<ResponseDTO> DeleteSchedule(RemoveScheduleDTO removeScheduleDTO, int scheduleId);
        Task<PagedResult<Schedule>> GetAllSchedulePagingAsync(int pageNumber, int pageSize);
        Task<PagedResult<Schedule>> GetAllSchedulePagingAsync_1(int pageNumber, int pageSize);
        Task<ResponseDTO> GetScheduleByIdAsync(int scheduleId);
        Task<ResponseDTO> ChangeStatusScheduleById(int scheduleId);
        Task<PagedResult<Schedule>> SearchScheduleByStartDateAsync(DateOnly startDate, int pageNumber, int pageSize);

    }
}
