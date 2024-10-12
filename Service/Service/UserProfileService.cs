using AutoMapper;
using BusinessObject;
using BusinessObject.Model;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UserProfileService: IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserProfileService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper= mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ResponseDTO> GetCurrentUserProfile()
        {
            try
            {
                // Lấy token từ header Authorization
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                // Giải mã token để lấy thông tin người dùng
                var claimsPrincipal = ValidateToken(token);

                // Lấy UserId từ claims
                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                // Tìm người dùng trong cơ sở dữ liệu
                var user = await _unitOfWork.UserRepository.GetUserByCurrentId(userId);
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                // Lấy hồ sơ người dùng và ánh xạ sang DTO
                var userProfileDto = _mapper.Map<UserProfileDTO>(user.UserProfile);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, "User profile retrieved successfully.", userProfileDto);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Không cho phép chênh lệch thời gian
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        }
    }
}
