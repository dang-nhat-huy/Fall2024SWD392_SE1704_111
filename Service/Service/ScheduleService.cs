using AutoMapper;
using BusinessObject;
using BusinessObject.Models;
using BusinessObject.Paging;
using BusinessObject.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        public IQueryable<Schedule> GetListSchedule()
        {
            var query = _unitOfWork.ScheduleRepository.GetAll();
            return query;
        }
        public async Task<ResponseDTO> GetListScheduleAsync()
        {
            try
            {
                var query = await _unitOfWork.ScheduleRepository.GetAllWithTwoInclude("Bookings", "ScheduleUsers").ToListAsync();

                if (query == null || !query.Any())
                {
                    return new ResponseDTO(Const.SUCCESS_CREATE_CODE, "Empty List");
                }

                // Sử dụng AutoMapper để ánh xạ danh sách
                var result = _mapper.Map<List<ScheduleDTO>>(query);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception e)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, e.Message);
            }
        }
    }
}
