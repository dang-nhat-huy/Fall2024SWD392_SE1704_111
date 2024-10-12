using AutoMapper;
using BusinessObject;
using BusinessObject.Models;
using BusinessObject.ResponseDTO;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UserProfileService: IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UserProfileService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper= mapper;
        }
        public async Task<ResponseDTO> GetAllUserProfile()
        {
            
            try
            {
                
                var userProfile = await _unitOfWork.userProfileRepository.getAllUserProfile();

                    var data= _mapper.Map<List<UserProfileDTO>>(userProfile);
                    return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, data);
        
            }
            catch (Exception ex) {
                return new ResponseDTO(Const.FAIL_READ_CODE, ex.Message, new List<UserProfileDTO>());
            }
          
        }

        public async Task<ResponseDTO> GetUserProfileByIdAsync(int id)
        {
            try
            {
                var userProfile = await _unitOfWork.userProfileRepository.GetByIdAsync(id);

                var result = _mapper.Map<UserProfileDTO>(userProfile);

                if (userProfile == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE,Const.FAIL_READ_MSG,new UserProfileDTO());
                }

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);

            }
            catch (Exception e)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, e.Message, new UserProfileDTO());
            }
        }

        public Task<ResponseDTO> UpdateUserProfileAsync(int id, ResponseDTO responseDTO)
        {
            throw new NotImplementedException();
        }
        

    }
}
