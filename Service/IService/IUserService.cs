using BadmintonRentingData;
using BusinessObject.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;

namespace Service.IService
{
    public interface IUserService
    {
        Task<ResponseDTO> GetAll();
        Task<ResponseDTO> Login(LoginRequestDTO request);
    }
}
