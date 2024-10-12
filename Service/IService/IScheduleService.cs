using BusinessObject.Models;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Service.IService
{
    public interface IScheduleService
    {
        Task<ResponseDTO> GetListScheduleAsync();
    }
}
