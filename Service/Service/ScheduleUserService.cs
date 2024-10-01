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

        public async Task<PagedResult<ScheduleUserDTO>> GetListScheduleUserAsync(int pageNumber, int pageSize)
        {
            // Lấy IQueryable từ repository
            var query = _unitOfWork.scheduleUserRepository.GetAllWithTwoInclude("Schedule", "User");

            // Sử dụng ProjectTo để ánh xạ thành IQueryable<ScheduleUserDTO>
            var resultQuery = _mapper.ProjectTo<ScheduleUserDTO>(query);

            // Gọi hàm phân trang
            return await Paging.GetPagedResultAsync(resultQuery, pageNumber, pageSize);
        }
    }
}
