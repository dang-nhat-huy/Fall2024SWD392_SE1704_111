using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.ResponseDTO.ResponseDTO;

namespace Repository.IRepository
{
    public interface IHairServiceRepository : IGenericRepository<HairService>
    {
        public Task<List<HairService?>> GetServiceByNameAsync(string serviceName);
        public Task<HairService> GetHairServiceById(int hairServiceId);
    }
}
