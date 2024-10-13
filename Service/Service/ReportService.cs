using AutoMapper;
using BusinessObject.Model;
using BusinessObject;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using static BusinessObject.ReportEnum;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Service
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ResponseDTO> CreateReportAsync(CreateReportDTO request)
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

                // Sử dụng AutoMapper 
                var report=_mapper.Map<Report>(request);

                report.StylistId=userId;
                // Lưu các thay đổi vào cơ sở dữ liệu
                var result = await _unitOfWork.reportRepository.CreateReportAsync(report);

                //if(result <= 0)
                //{
                //    return new ResponseDTO(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, "Create Report Failed");
                //}

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message, ex);
            }
        }
    }
}
