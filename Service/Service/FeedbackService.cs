using AutoMapper;
using BusinessObject;
using BusinessObject.RequestDTO;
using BusinessObject.ResponseDTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.RequestDTO.RequestDTO;
using static BusinessObject.FeedbackStatusEnum;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Service.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService _jWTService;
        public FeedbackService(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jWTService = jWTService;
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
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        }
        public async Task<ResponseDTO> CreateFeedbackAsync(FeedbackRequestDTO request)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                var claimsPrincipal = ValidateToken(token);

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                var feedback = _mapper.Map<Feedback>(request);
                feedback.UserId = userId;
                feedback.CreateBy = user.UserName;
                feedback.CreateDate = DateTime.Now;

                var result = await _unitOfWork.FeedbackRepository.CreateFeedbackAsync(feedback);

                return new ResponseDTO(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdateFeedbackAsync(FeedbackRequestDTO request, int feedbackId)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                var claimsPrincipal = ValidateToken(token);

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackById(feedbackId);
                if (feedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Feedback not found");
                }

                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                _mapper.Map(request, feedback);

                feedback.UpdateBy = user.UserName;
                feedback.UpdateDate = DateTime.Now;

                var result = await _unitOfWork.FeedbackRepository.UpdateFeedbackAsync(feedback);

                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_UPDATE_CODE, "Update feedback failed");
                }

                return new ResponseDTO(Const.SUCCESS_UPDATE_CODE, "Update feedback succeeded");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> ChangeFeedbackStatusAsync(int feedbackId, FeedbackStatusEnum newStatus)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Token is missing.");
                }

                var claimsPrincipal = ValidateToken(token);

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found in token.");
                }

                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackById(feedbackId);
                if (feedback == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Feedback not found");
                }

                var user = await _jWTService.GetCurrentUserAsync();
                if (user == null)
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "User not found.");
                }

                feedback.Status = newStatus;
                feedback.UpdateBy = user.UserName;
                feedback.UpdateDate = DateTime.Now;

                var result = await _unitOfWork.FeedbackRepository.UpdateFeedbackAsync(feedback);

                if (result == null)
                {
                    return new ResponseDTO(Const.FAIL_UPDATE_CODE, "Update feedback status failed");
                }

                return new ResponseDTO(Const.SUCCESS_UPDATE_CODE, "Update feedback status succeeded");
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<ResponseDTO> GetFeedbackListAsync()
        {
            try
            {
                // Retrieve all feedbacks from the repository
                var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();

                if (feedbacks == null || !feedbacks.Any())
                {
                    return new ResponseDTO(Const.FAIL_READ_CODE, "Empty Feedback List");
                }

                // Map feedback entities to FeedbackDTOs
                var feedbackDTOs = _mapper.Map<List<FeedbackResponseDTO>>(feedbacks);

                return new ResponseDTO(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, feedbackDTOs);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
