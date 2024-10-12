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
    public interface IUserProfileService
    {
        Task<ResponseDTO> GetAllUserProfile();
        Task<ResponseDTO> GetUserProfileByIdAsync(int id);
        Task<ResponseDTO> UpdateUserProfileAsync(UpdateUserProfileDTO request);
        Task<ResponseDTO> GetCurrentUserProfile();
    }
}
