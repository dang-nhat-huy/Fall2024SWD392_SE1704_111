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
    public interface IHairServiceService
    {
        Task<ResponseDTO> GetListServicesAsync();
        Task<ResponseDTO> GetServiceByIdAsync(int id);
        Task<ResponseDTO> GetServiceByNameAsync(string name);
       
        Task<ResponseDTO> CreateServiceAsync(CreateServiceDTO request);
        Task<ResponseDTO> ChangeServiceStatusAsync(RemoveServiceDTO request, int servicetId);
        Task<ResponseDTO> UpdateServiceAsync(UpdateServiceDTO request, int serviceId);
    }
}
