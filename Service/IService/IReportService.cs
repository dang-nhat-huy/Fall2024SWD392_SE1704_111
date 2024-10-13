using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.IService
{
    public interface IReportService
    {
        Task<ResponseDTO> CreateReportAsync(CreateReportDTO request);

        Task<ResponseDTO> UpdateReportAsync(UpdateReportDTO request, int bookingId);
    }
}
