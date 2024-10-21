using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.IService
{
    public interface IVoucherService
    {
        Task<ResponseDTO> GetListVoucherAsync();
        Task<ResponseDTO> ChangeStatusVoucherById(int voucherId);
        Task<ResponseDTO> UpdateVoucherById(int voucherId, UpdateVoucherDTO request);
        Task<ResponseDTO> CreateVoucherAsync(CreateVoucherDTO request);
    }
}
