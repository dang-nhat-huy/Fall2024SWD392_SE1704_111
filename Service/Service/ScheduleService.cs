using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.Paging;
using BusinessObject.ResponseDTO;
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

        public async Task<PagedResult<Schedule>> GetListScheduleAsync(int pageNumber, int pageSize)
        {
            var query = _unitOfWork.ScheduleRepository.GetAll();
            return await Paging.GetPagedResultAsync(query.AsQueryable(), pageNumber, pageSize);
        }
    }
}
