using AutoMapper;
using BusinessObject.Model;
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
    public class ScheduleUserService : IScheduleUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ScheduleUserService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        public async Task<List<ScheduleUserDTO>> GetListScheduleUserAsync()
        {
            // Lấy IQueryable từ repository
            var listQuery = await _unitOfWork.ScheduleUserRepository.GetListScheduleByRoleAsync();

            // Sử dụng ProjectTo để ánh xạ thành List<ScheduleUserDTO>
            var resultList = await _mapper.ProjectTo<ScheduleUserDTO>(listQuery).ToListAsync();

            return resultList; // Trả về danh sách ScheduleUserDTO
        }
    }
}
